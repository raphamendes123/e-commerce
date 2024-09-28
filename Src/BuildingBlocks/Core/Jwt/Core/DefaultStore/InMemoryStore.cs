﻿using System.Collections.ObjectModel;
using Core.Security.Core.Interfaces;
using Core.Security.Core.Model;

namespace Core.Security.Core.DefaultStore;

internal class InMemoryStore : IJsonWebKeyStore
{
    internal const string DefaultRevocationReason = "Revoked";
    private static readonly List<KeyMaterial> _store = new();
    private readonly SemaphoreSlim _slim = new(1);
    public Task Store(KeyMaterial keyMaterial)
    {
        _slim.Wait();
        _store.Add(keyMaterial);
        _slim.Release();

        return Task.CompletedTask;
    }

    public Task<KeyMaterial> GetCurrent()
    {
        return Task.FromResult(_store.OrderByDescending(s => s.CreationDate).FirstOrDefault());
    }

    public async Task Revoke(KeyMaterial keyMaterial, string reason = null)
    {
        if (keyMaterial == null)
            return;
        var revokeReason = reason ?? DefaultRevocationReason;
        keyMaterial.Revoke(revokeReason);
        var oldOne = _store.Find(f => f.Id == keyMaterial.Id);
        if (oldOne != null)
        {
            var index = _store.FindIndex(f => f.Id == keyMaterial.Id);
            await _slim.WaitAsync();
            _store.RemoveAt(index);
            _store.Insert(index, keyMaterial);
            _slim.Release();
        }
    }

    public Task<ReadOnlyCollection<KeyMaterial>> GetLastKeys(int quantity)
    {
        return Task.FromResult(
            _store
                .OrderByDescending(s => s.CreationDate)
                .Take(quantity).ToList().AsReadOnly());
    }

    public Task<KeyMaterial> Get(string keyId)
    {
        return Task.FromResult(_store.FirstOrDefault(w => w.KeyId == keyId));
    }

    public Task Clear()
    {
        _store.Clear();
        return Task.CompletedTask;
    }
}