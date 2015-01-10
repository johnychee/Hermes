using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public partial class TicketSla
    {
        // pracovní dny
        private static DayOfWeek[] workDays = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
        // v kolik hodin začíná pracovní den
        private static int bussinesDayStartAt_hour = 7;
        // v kolik končí pracovní den
        private static int bussinesDayEndAt_hour = 19;
        // časový posun
        public TimeSpan? timeShift { get; set; }

        public string RTremaining_toString()
        {
            return timeSpan_toLightString(this.RT_expiresAt - DateTime.UtcNow);
        }
        public string FTremaining_toString()
        {
            return timeSpan_toLightString(this.FT_expiresAt - DateTime.UtcNow);
        }
        public string FT3lvlRemaining_toString()
        {
            return timeSpan_toLightString(this.FT3lvl_expiresAt - DateTime.UtcNow);
        }

        // spočítá konec sla
        // duration == null => End of 2nd bussines day
        public DateTime? countEndAt(DateTime? startAt, TimeSpan? duration)
        {
            // iniciace - nastaví časový posun
            timeShift = timeShift ?? TimeSpan.FromHours(this.Ticket.Author.Region.UTCtimeShift);

            // vyřešit časový posun
            DateTime? countingStart = timeShift_begin(startAt);

            // End of 2nd bussines day
            if (duration == null)
                // přesunutí na další den
                // nalezení konce pracovního dne
                // časový posun
                return timeShift_end(nextDown(nextDown(startAt.Value)));

            // nalezne začátek SLA
            DateTime? up = nextUp(countingStart);
            // nalezne první sestupnou hranu
            DateTime? down = nextDown(up);
            // zpočítá zbývající čas do konce pracovní doby
            TimeSpan tillDayEnds = down.Value - up.Value;

            // odstraní přetečení dne
            while (duration > tillDayEnds)
            {
                duration = duration.Value - tillDayEnds;

                up = nextUp(down);
                down = nextDown(up);
                tillDayEnds = down.Value - up.Value;
            }

            // dopočítá hodiny ve dni
            DateTime? countingEnd = up.Value + duration;

            // navrátí časový posun
            return timeShift_end(countingEnd);
        }


        // řeší časový posun
        private DateTime? timeShift_begin(DateTime? dateTime)
        {
            return dateTime + this.timeShift;
        }
        private DateTime? timeShift_end(DateTime? dateTime)
        {
            return dateTime - this.timeShift;
        }

        // hledá následující náběžné a sestupné hrany
        private DateTime? nextUp(DateTime? dateTime)
        {
            // pokud dateTime je uprostřed pracovní doby
            if (workDays.Contains(dateTime.Value.DayOfWeek) && dateTime.Value.Hour >= bussinesDayStartAt_hour && dateTime.Value.Hour < bussinesDayEndAt_hour)
                return dateTime;

            // dateTime je mimo pracovní dobu -> nalezení nejbližší náběžné hrany
            // vždy bude mít up na BeginOfBussinesDay
            DateTime? up;

            // nastavení času
            // čas je před zahájením pracovní doby -> start v tomto dni
            if (dateTime.Value.Hour < bussinesDayStartAt_hour)
                up = dateTime.Value.Date + TimeSpan.FromHours(bussinesDayStartAt_hour);
            // čas je po pracovní době -> start až příští den
            else // countingStart.Value.Hour >= bussinesDayEndAt_hour
                up = dateTime.Value.Date + TimeSpan.FromDays(1) + TimeSpan.FromHours(bussinesDayStartAt_hour);

            // nalezení pracovního dne
            while (!isWorkDay(up))
            {
                up = up.Value + TimeSpan.FromDays(1);
            }

            return up;
        }
        private DateTime? nextDown(DateTime? dateTime)
        {
            // set down to this day end
            DateTime? down = dateTime.Value.Date + TimeSpan.FromHours(bussinesDayEndAt_hour);

            // time is after end => next day
            if (dateTime >= down)
                down = down.Value + TimeSpan.FromDays(1);

            // find workday
            while (!isWorkDay(down))
            {
                down = down.Value + TimeSpan.FromDays(1);
            }

            return down;
        }

        // je daný den pracovní?
        private bool isWorkDay(DateTime? dateTime)
        {
            // je možno přidat svátky
            return workDays.Contains(dateTime.Value.DayOfWeek);
        }

        private string timeSpan_toLightString(TimeSpan? time)
        {
            // dny
            if (time.Value.Days > 1 || time.Value.Days < -1)
                return time.Value.Days + " days";
            if (time.Value.Days == 1 || time.Value.Days == -1)
                return time.Value.Days + " days " + time.Value.Hours + " hours";

            // hodiny
            if (time.Value.Hours > 3 || time.Value.Hours < -3)
                return time.Value.Hours + " hours";
            if (time.Value.Hours > 0 || time.Value.Hours < 0)
                return time.Value.Hours + " hours" + time.Value.Minutes + " minutes";

            // minuty
            return time.Value.Minutes + " minutes";

        }
    }
}