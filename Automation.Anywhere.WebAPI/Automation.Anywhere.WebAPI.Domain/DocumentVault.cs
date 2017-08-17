using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Anywhere.WebAPI.Domain
{
    public class DocumentVault
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Owner { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<Document> Documents { get; set; }

        public DocumentVault()
        {
            Documents = new HashSet<Document>();
        }
    }
}
