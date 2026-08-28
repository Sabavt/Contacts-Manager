namespace CRUDTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            MyMath mm = new MyMath();
            int value1 = 4, value2 = 5;
            int expected = 9;

            //Act
            int actual = mm.Add(value1, value2);

            //Asset
            Assert.Equal(expected, actual);
        }
    }
}
