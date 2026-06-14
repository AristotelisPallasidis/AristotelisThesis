using System.Threading.Tasks;
using AristotelisThesis.Domain.Models;

namespace AristotelisThesis.Domain.Services
{
    /// <summary>
    /// Builds an <see cref="AttendanceStatistics"/> read-model for a student by
    /// aggregating their persisted <see cref="SessionHistory"/> records.
    /// </summary>
    public interface IStatisticsService
    {
        Task<AttendanceStatistics> GetForStudent(int studentId);
    }
}
