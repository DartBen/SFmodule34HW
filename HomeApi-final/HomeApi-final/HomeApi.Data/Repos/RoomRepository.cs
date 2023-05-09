using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using HomeApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Получение списка комнат
        /// </summary>
        /// <returns></returns>
        public async Task<Room[]> GetRooms()
        {
            return await _context.Rooms
                .ToArrayAsync();
        }

        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Обновить информацию о комнате
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task UpdateRoom(Room room)
        {

            var existedRoom = await _context.Rooms.Where(r => r.Name == room.Name).FirstOrDefaultAsync();

            Console.WriteLine(room.Area + "\n" + existedRoom.Area);

            if (existedRoom == null) return;

            existedRoom.Area = room.Area;
            existedRoom.GasConnected = room.GasConnected;
            existedRoom.Voltage = room.Voltage;


            _context.Rooms.Update(existedRoom);

            await _context.SaveChangesAsync();
        }
    }
}