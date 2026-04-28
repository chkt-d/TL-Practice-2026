public class Program
{
    static void Main()
    {
        Order order = new Order();

        order.ProductName = ReadString( "Введите название товара:" );
        order.Count = ReadPositiveInt( "Введите количество товара:" );
        order.UserName = ReadString( "Введите ваше имя:" );
        order.Address = ReadString( "Введите адрес доставки:" );

        bool isConfirmed = AskForConfirmation( order );

        if ( isConfirmed )
        {
            PrintSuccessMessage( order );
        }
        else
        {
            Console.WriteLine( "Заказ не оформлен." );
        }
    }

    private static string ReadString( string message )
    {
        while ( true )
        {
            Console.Write( $"{message} " );
            string input = Console.ReadLine();

            if ( !string.IsNullOrWhiteSpace( input ) )
            {
                return input.Trim();
            }

            Console.WriteLine( "Поле не должно быть пустым." );
        }
    }

    private static int ReadPositiveInt( string message )
    {
        while ( true )
        {
            Console.Write( $"{message} " );
            string input = Console.ReadLine();

            if ( int.TryParse( input, out int value ) && value > 0 )
            {
                return value;
            }

            Console.WriteLine( "Введите целое число больше 0." );
        }
    }

    private static bool AskForConfirmation( Order order )
    {
        while ( true )
        {
            Console.WriteLine( $"Здравствуйте, {order.UserName}, вы заказали {order.Count} {order.ProductName} на адрес {order.Address}, все верно? [Y/n]" );

            string answer = Console.ReadLine();

            if ( string.IsNullOrWhiteSpace( answer ) )
            {
                return true;
            }

            answer = answer.Trim().ToLower();

            if ( answer == "да" || answer == "y" || answer == "yes" )
            {
                return true;
            }

            if ( answer == "нет" || answer == "n" || answer == "no" )
            {
                return false;
            }

            Console.WriteLine( "Нажмите Enter для подтверждения или введите n/нет для отмены." );
        }
    }

    private static void PrintSuccessMessage( Order order )
    {
        DateTime deliveryDate = DateTime.Today.AddDays( 3 );

        Console.WriteLine(
            $"{order.UserName}! Ваш заказ {order.ProductName} в количестве {order.Count} оформлен! Ожидайте доставку по адресу {order.Address} к {deliveryDate:dd.MM.yyyy}" );
    }
}

class Order
{
    public string ProductName = "";

    public int Count = 0;

    public string UserName = "";

    public string Address = "";
}