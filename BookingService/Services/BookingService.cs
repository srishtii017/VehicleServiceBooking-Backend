using BookingService.DTO;
using BookingService.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http;

namespace BookingService.Services
{
    public class BookingServiceImp : IBookingService
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpclientfactory;

        public BookingServiceImp(AppDbContext context,IHttpClientFactory httpclientfactory)
        {
            _context = context;
            _httpclientfactory = httpclientfactory;
        }

        public async Task<Booking> CreateBookingAsync(int userId, CreateBookingDTO dto)
        {
            try
            {

                if (await IsDupliacte(dto.VehicleNo,dto.ServiceDate))
                {
                    return null;
                }


                var client = _httpclientfactory.CreateClient();
                client.BaseAddress = new Uri("http://localhost:5226");

                var response = await client.GetAsync($"/api/User/{userId}");

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<User>>();

                string IdPrefix = result.Data.Name.Length >= 3 ? result.Data.Name.Substring(0, 3) : result.Data.Name;
                int nextNumber = _context.Bookings.Count() + 1;
                string bookingId = $"{IdPrefix}{nextNumber.ToString("D3")}";

                var booking = new Booking
                {
                    BookingId = bookingId,
                    UserId = userId,
                    CustomerName = dto.CustomerName,
                    VehicleName = dto.VehicleName,
                    VehicleNo = dto.VehicleNo,
                    VehicleType = dto.VehicleType,
                    ServiceCenterId = dto.ServiceCenterId,
                    Phone_No = result.Data.Phone,
                    Email = result.Data.Email,
                    Address = result.Data.Address,
                    ServiceType = dto.ServiceType,
                    CreatedDate = DateTime.Now,
                    ServiceDate = dto.ServiceDate,
                    Status = BookingStatus.Pending.ToString()
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return booking;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating booking: {ex.Message}");
            }
        }

        public async Task<Booking?> GetBookingByIdAsync(int id, int userId)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null || booking.UserId != userId)
                    return null;

                return booking;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching booking: {ex.Message}");
            }
        }

        public async Task<Booking?> UpdateBookingAsync(int userId,UpdateBookingDTO dto)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(dto.bookingId);

                if (booking == null || booking.UserId != userId)
                    return null;

                if (booking.Status != BookingStatus.Pending.ToString())
                    return null;

                if (await IsDupliacte(dto.VehicleNo,dto.ServiceDate))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(dto.ServiceType) && !string.IsNullOrEmpty(dto.VehicleType)){
                    booking.ServiceType = dto.ServiceType;
                    booking.VehicleType = dto.VehicleType;
                    booking.VehicleNo = dto.VehicleNo;
                }
                

                if (dto.ServiceDate != default && dto.ServiceDate >= DateOnly.FromDateTime(DateTime.Today))
                    booking.ServiceDate = dto.ServiceDate;

                await _context.SaveChangesAsync();

                return booking;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating booking: {ex.Message}");
            }
        }

        public async Task<bool> CancelBookingAsync(string id, int userId)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);

                if (booking == null || booking.UserId != userId)
                    return false;

                if (booking.Status == BookingStatus.Completed.ToString() ||
                    booking.Status == BookingStatus.Cancelled.ToString())
                    return false;

                booking.Status = BookingStatus.Cancelled.ToString();

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error cancelling booking: {ex.Message}");
            }
        }

        public async Task<List<GetBookingDTO>> GetUserBookingsAsync(int userId)
        {
            try
            {
                var agent = await _context.Bookings
                    .Where(b => b.UserId == userId)
                    .Select(b => new GetBookingDTO
                    {
                        BookingId = b.BookingId,
                        VehicleNo = b.VehicleNo,
                        VehicleType = b.VehicleType,
                        ServiceCenterId = b.ServiceCenterId,
                        ServiceType = b.ServiceType,
                        ServiceDate = b.ServiceDate,
                        Status = b.Status,
                        CreatedDate = b.CreatedDate
                    })
                    .ToListAsync();
                return agent;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching user bookings: {ex.Message}");
            }
        }

        private async Task<bool> IsDupliacte(string vehicleNo,DateOnly date)
        {
            var vehicleN = vehicleNo.ToLower().Trim();

            return await _context.Bookings.
                AnyAsync(b => b.VehicleNo.ToLower() == vehicleN &&
                         b.ServiceDate == date);
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            try
            {
                return await _context.Bookings.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching bookings : {ex.Message}");
            }
        }


    }
}