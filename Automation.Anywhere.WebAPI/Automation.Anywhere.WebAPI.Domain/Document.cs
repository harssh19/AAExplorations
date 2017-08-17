using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.Anywhere.WebAPI.Domain
{
    public class Document
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string Author { get; set; }
        public string OnlineURL { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }

        public int DocumentVaultID { get; set; }
        public virtual DocumentVault Vault { get; set; }

        public Document()
        {
        }
    }
}
