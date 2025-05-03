using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicacaoDiariaApp.Models
{
    public class Medicamento
    {
        [PrimaryKey, AutoIncrement]
        public int IdMedicamento { get; set; }

        public string Nome { get; set; }

        public string Indicacao { get; set; }
    }
}
