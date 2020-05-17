using AutoMapper;
using FutsalSystem.Models;
using FutsalSystem.Models.DTO.User;
using FutsalSystem.Repository.Interface;
using FutsalSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FutsalSystem.Services
{
    public class UserService : IUserService
    {
        protected readonly IRepository _repository;
        protected readonly IMapper _mapper;

        public UserService(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetEntities()
        {
            IQueryable<User> users = await _repository.QueryAsync<User>();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetEntityById(int userId)
        {
            User user = await _repository.QueryByIdAsync<User>(userId);
            if (user != null)
                return _mapper.Map<UserDTO>(user);

            throw new System.InvalidOperationException($"User with id {userId} was not found.");
        }

        public async Task<UserDTO> CreateEntity(UserDTO userDTO)
        {
            User user = _mapper.Map<User>(userDTO);
            user.Id = 0;
            var createdUser = await _repository.CreateAsync(user);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<UserDTO> UpdateEntity(UserDTO userDTO)
        {
            User user = _mapper.Map<User>(userDTO);
            await _repository.UpdateAsync(userDTO.Id, user);
            return userDTO;
        }

        public async Task DeleteEntity(int userId)
        {
            User user = await _repository.QueryByIdAsync<User>(userId);
            if (user == null)
                throw new InvalidOperationException($"User with id {userId} not found.");

            await _repository.DeleteAsync<User>(userId);
        }
    }
}
