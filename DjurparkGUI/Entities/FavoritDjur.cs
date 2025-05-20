using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjurparkGUI.Entities
{
    // Denna klass representerar en många-till-många-relation
    // mellan Besökare och Djur – vilka djur en besökare gillar.
    public class FavoritDjur
    {
        public int BesökareId { get; set; }
        public Besökare Besökare { get; set; }

        public int DjurId { get; set; }
        public Djur Djur { get; set; }
    }
}