using MyApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Service
{
    public class ChiffreService
    {

        public Chiffre generation()
        {
            Chiffre chiffre = new Chiffre();
            chiffre.entier = new Random().Next(1, 100);
            return chiffre;
        }
    }
}
