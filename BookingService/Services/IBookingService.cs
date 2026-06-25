using BookingService.DTO;
using BookingService.Model;

namespace BookingService.Services
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(int userId, CreateBookingDTO dto);
        Task<Booking?> GetBookingByIdAsync(int id, int userId);
        Task<Booking?> UpdateBookingAsync(int userId,UpdateBookingDTO dto);
        Task<bool> CancelBookingAsync(string id, int userId);
        Task<List<GetBookingDTO>> GetUserBookingsAsync(int userId);
        Task<List<Booking>> GetAllBookings();

    }
}
