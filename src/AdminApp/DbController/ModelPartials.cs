using System;
using System.ComponentModel;
using Common;
using Common.Interfaces;

namespace DbController
{
    public partial class Whore : IIdentable
    {

        public override string ToString()
        {
            return FirstName + " " + LastName;
            
        }
    }

    public partial class Pimp : IIdentable
    {
        public override string ToString()
        {
            return Name;
        }
    }

}
