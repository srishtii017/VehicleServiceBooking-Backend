using BookingService.DTO;
using BookingService.Model;

namespace BookingService.Services
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(int userId, CreateBookingDTO dto,string token);
        Task<Booking?> GetBookingByIdAsync(string id);
        Task<Booking?> UpdateBookingAsync(int userId,UpdateBookingDTO dto);
        Task<bool> CancelBookingAsync(string id, int userId);
        Task<List<GetBookingDTO>> GetUserBookingsAsync(int userId);
        Task<List<Booking>> GetAllBookings();
        Task<bool> UpdateBookingStatusAsync(string bookingId, string newStatus);

    }
}
