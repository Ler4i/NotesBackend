
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        Task<UserInformationBlo> RegisterWithPhone(string numberPrefix, string number, string password);//регистрация в ()-запрос
        Task<UserInformationBlo> AuthWithPhone(string numberPrefix, string number, string password);//авторизация через телефон
        Task<UserInformationBlo> AuthWithEmail(string email, string password);//авторизация через мыло
        Task<UserInformationBlo> AuthWithLogin(string login, string password);
        Task<UserInformationBlo> Get(int userId);
        Task<UserInformationBlo> Update(UserUpdateBlo userUpdateBlo);//put
        Task<bool> DoesExist(string numberPrefix, string number);//занят ли номер телефона
    }
}