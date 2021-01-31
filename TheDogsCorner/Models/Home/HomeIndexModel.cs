using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TheDogsCorner.DAL;
using TheDogsCorner.Repository;

namespace TheDogsCorner.Models.Home
{
    public class HomeIndexModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        TheDogsCornerEntities context = new TheDogsCornerEntities();
        public IPagedList<Produkt> ListOfProducts { get; set; }
        public IPagedList<Kategori> ListOfCategories { get; set; }

        public HomeIndexModel CreateModel(string search, int pageSize, int? page)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@search", search??(object)DBNull.Value)
            };

            IPagedList<Produkt> data = context.Database.SqlQuery<Produkt>("GetBySearch @search", param).ToList().ToPagedList(page ?? 1, pageSize);
            return new HomeIndexModel
            {
                ListOfProducts = data
            };
        }

        public HomeIndexModel CategoryModel(string search, int pageSize, int? page)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@search", search??(object)DBNull.Value)
            };

            IPagedList<Kategori> data = context.Database.SqlQuery<Kategori>("SearchCategory @search", param).ToList().ToPagedList(page ?? 1, pageSize);
            return new HomeIndexModel
            {
                ListOfCategories = data
            };
        }
    }
}