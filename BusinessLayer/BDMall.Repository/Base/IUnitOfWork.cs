﻿namespace BDMall.Repository
{
    public interface IUnitOfWork : IDependency
    {
        /// <summary>
        /// 业务操作人员
        /// </summary>
        
        MallDbContext DataContext { get; }

        //MallDbContext GetNewDataContext();

        //MallDbContext SetConnectionString(string connectionString);

        /// <summary>
        /// 将操作提交到数据库，
        /// </summary>
        int Submit();


        /// <summary>
        /// 是否统一提交，这只是在具体的repository类中的SaveChanges方法里用到的
        /// 默认为false，即默认为不统一提交到数据库
        /// </summary>
        /// <returns></returns>
        bool IsUnitSubmit { get; set; }

        Task<int> SubmitAsync();
    }
}
