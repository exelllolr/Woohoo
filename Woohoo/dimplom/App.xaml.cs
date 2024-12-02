
using System.Windows;
using dimplom.Model;



namespace dimplom
{
    public partial class App : Application
    {
        public static WooHooEntities Db = new WooHooEntities();
        public static Users loggedUser;
    }
}

