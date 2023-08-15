using System.Collections.Generic;
using DataLayer.AdminEntities.Category;
using DataLayer.AdminEntities.Logo;
using DataLayer.AdminEntities.SocialNetwork;
namespace gajabi.Models
{
    public static class menu
    {
        public static List<Tb_Category> cat = new List<Tb_Category>();
        public static int  resiver,sender,total_user,new_order,new_comment,idrezerv=0;
        public static string logo,Title,img,favicon,enemad;
        public static string Tel,Insta,Face,Whts,Twit,site;
        public static string Richat,zarincode,phone,sms;
        public static string FullTextBlo;
        
        public static int order_count;
        public static List<int> shop = new List<int>();
        public static List<string> find { get; set; }

        public static string newp { get; set; }
         public static string watsap { get; set; }
          public static string contactus { get; set; }
          public static string app { get; set; }
        
        
        public static string majicp { get; set; }

        public static string PriceGold { get; set; }

        
        
        
        
        // public static string social ,Instagram ;
    }
}