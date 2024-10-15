using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gym_management;
using NUnit.Framework;

namespace GymManagement.Test
{
    [TestFixture]
    public class WalkingTests
    {
        private walking walking;

        [SetUp]
        public void Setup()
        {
            walking = new walking("test_user");
        }

        [Test]
        public void TestCalculateCalorieBurn_LowSpeed()
        {
            
            int stepsCount = 10000;
            double distanceKm = 1.0;
            int durationMinutes = 60;
            double userweight = 70.0; 

            
            double result = walking.CalculateCalorieBurn(stepsCount, distanceKm, durationMinutes, userweight);

            
            Assert.AreEqual(140.0, result, 0.1); 
        }

        [Test]
        public void TestCalculateCalorieBurn_ModerateSpeed()
        {
            
            int stepsCount = 10000;
            double distanceKm = 5.0;
            int durationMinutes = 60;
            double userweight = 70.0; 

            
            double result = walking.CalculateCalorieBurn(stepsCount, distanceKm, durationMinutes, userweight);


            Assert.AreEqual(280.0, result, 0.1); 
        }

        [Test]
        public void TestCalculateCalorieBurn_HighSpeed()
        {
            
            int stepsCount = 10000;
            double distanceKm = 10.0;
            int durationMinutes = 60;
            double userweight = 70.0; 

           
            double result = walking.CalculateCalorieBurn(stepsCount, distanceKm, durationMinutes, userweight);

           
            Assert.AreEqual(280.0, result, 0.1); 
        }
    }
}


