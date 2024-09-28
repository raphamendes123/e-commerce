﻿using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Cryptography;
using System.Text;

namespace Front.MVC.Extensions
{
    public static class RazorHelpers
    {
        public static string MensagemEstoque(this RazorPage page, int quantidade)
            => quantidade > 0 ? $"Apenas {quantidade} em estoque!" : "Produto esgotado!";

        public static string FormatoMoeda(this RazorPage page, decimal valor)
            => valor > 0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", valor) : "Gratuito";

        public static string UnidadesPorProduto(this RazorPage page, int unidades)
            => unidades > 1 ? $"{unidades} unidades" : $"{unidades} unidade";

        // para criar seu avatar publicamente use: https://br.gravatar.com/
        public static string HashEmailForGravatar(this RazorPage page, string email)
        {
            if (string.IsNullOrEmpty(email))
                return string.Empty;

            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));
            var sBuilder = new StringBuilder();

            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string SelectOptionsPorQuantidade(this RazorPage page, int quantidade, int valorSelecionado = 0)
        {
            var sb = new StringBuilder();

            for (var i = 1; i <= quantidade; i++)
            {
                var selected = "";
                if (i == valorSelecionado) selected = "selected";
                sb.Append($"<option {selected} value='{i}'>{i}</option>");
            }

            return sb.ToString();
        }

        public static string FormatMoney(this RazorPage page, decimal valor)
        {
            return FormatMoney(valor);
        }

        private static string FormatMoney(decimal valor)
        {
            return string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:C}", valor);
        }

        public static string StockMessage(this RazorPage page, int quantidade)
        {
            return quantidade > 0 ? $"Only {quantidade} at stock!" : "No stock!";
        }

        public static string UnityByProduct(this RazorPage page, int unidades)
        {
            return unidades > 1 ? $"{unidades} units" : $"{unidades} unit";
        }

        public static string SelectOptionsByQuantity(this RazorPage page, int quantidade, int valorSelecionado = 0)
        {
            var sb = new StringBuilder();
            for (var i = 1; i <= quantidade; i++)
            {
                var selected = "";
                if (i == valorSelecionado) selected = "selected";
                sb.Append($"<option {selected} value='{i}'>{i}</option>");
            }

            return sb.ToString();
        }

        public static string UnitByProductAmount(this RazorPage page, int unidades, decimal valor)
        {
            return $"{unidades}x {FormatMoney(valor)} = Total: {FormatMoney(valor * unidades)}";
        }

        public static string ShowStatus(this RazorPage page, int status)
        {
            var statusMensagem = "";
            var statusClasse = "";

            switch (status)
            {
                case 1:
                    statusClasse = "info";
                    statusMensagem = "Processing";
                    break;
                case 2:
                    statusClasse = "primary";
                    statusMensagem = "Approved";
                    break;
                case 3:
                    statusClasse = "danger";
                    statusMensagem = "Refused";
                    break;
                case 4:
                    statusClasse = "success";
                    statusMensagem = "Delivered";
                    break;
                case 5:
                    statusClasse = "warning";
                    statusMensagem = "Canceled";
                    break;

            }

            return $"<span class='badge badge-{statusClasse}'>{statusMensagem}</span>";
        }
    }
}
