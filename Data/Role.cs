using Spark.Library.Database;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorMinimalApis.Data
{
    [Table("Roles")]
    public class Role : BaseModel
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
