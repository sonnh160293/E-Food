namespace FoodOnline.Domain.Common
{
    public static class TimeSlotHelper
    {


        private static void GenerateTimeSlotsForRange(TimeSpan startTime, TimeSpan endTime, List<string> timeSlots)
        {
            while (startTime <= endTime)
            {
                timeSlots.Add(startTime.ToString(@"hh\:mm"));
                startTime = startTime.Add(new TimeSpan(0, 30, 0)); // Increment by 30 minutes
            }
        }

        public static List<string> GenerateTimeSlots()
        {
            var timeSlots = new List<string>();

            // Lunch Time Slots
            for (var hour = 10; hour <= 14; hour++)
            {
                timeSlots.Add($"{hour}:00");
                if (hour != 14) timeSlots.Add($"{hour}:30");
            }

            // Dinner Time Slots
            for (var hour = 17; hour <= 20; hour++)
            {
                timeSlots.Add($"{hour}:00");
                if (hour != 20) timeSlots.Add($"{hour}:30");
            }

            return timeSlots;
        }
    }

}
