using Core.Security.Core.Model;
using Microsoft.EntityFrameworkCore; 

namespace Core.Security.Jwt.EntityFrameworkCore
{
    public interface ISecurityKeyContext
    {
        /// <summary>
        /// A collection of <see cref="T:.Security.Jwt.Core.Model.KeyMaterial" />
        /// </summary>
        /// 

        DbSet<KeyMaterial> SecurityKeys { get; set; }
    }
}
