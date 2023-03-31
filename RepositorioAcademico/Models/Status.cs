using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RepositorioAcademico.Models
{
    public class Status
    {
        private int tipo;
        private string mensaje;

        public int Tipo { get => tipo; set => tipo = value; }
        public string Mensaje { get => mensaje; set => mensaje = value; }
    }
}