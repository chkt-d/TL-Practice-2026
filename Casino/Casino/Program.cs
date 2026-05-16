double balance = 0;
bool isGameFinished = false;

PrintHeader();
Console.WriteLine( "Press Enter to start!" );
Console.ReadLine();

while ( !isGameFinished )
{
    PrintMenu();

    string option = Console.ReadLine();
    Console.WriteLine();
    OptionHandleResult result = HandleOptions( option );
    if ( result != OptionHandleResult.Success )
    {
        PrintError( result );
    }
}

void PrintError( OptionHandleResult result )
{
    switch ( result )
    {
        case OptionHandleResult.InvalidOption:
            Console.WriteLine( "Invalid option." );
            break;

        case OptionHandleResult.InvalidDepositValue:
            Console.WriteLine( "Invalid deposit value." );
            break;

        case OptionHandleResult.InvalidBetValue:
            Console.WriteLine( "Invalid bet value." );
            break;
    }

    Console.WriteLine();
}

void PrintHeader()
{
    string header = """
 ####    ####    ####  # #   #  #### 
#    #  #    #  #      # ##  # #    #
#       #    #   ####  # # # # #    #
#       ######      #  # #  ## #    #
#    #  #    #  #   #  # #   # #    #
 ####   #    #   ###   # #   #  #### 
""";

    Console.WriteLine( header );
    Console.WriteLine();
    Console.WriteLine( "Rules:" );
    Console.WriteLine( "1. Enter your bet." );
    Console.WriteLine( "2. A random number from 1 to 20 is generated." );
    Console.WriteLine( "3. Winning numbers are 18, 19, and 20." );
    Console.WriteLine( "4. On loss, your bet is taken." );
    Console.WriteLine( "5. On win, the prize is calculated based on the random number." );
}

void PrintMenu()
{
    List<string> menuOptions = [
        "1. Deposit",
        "2. Show balance",
        "3. Gamble!",
        "4. Exit" ];

    foreach ( string menuOption in menuOptions )
    {
        Console.WriteLine( menuOption );
    }

    Console.Write( "What do you want to do? " );
}

OptionHandleResult HandleOptions( string option )
{
    return option switch
    {
        "1" => MakeDeposit(),
        "2" => ShowBalance(),
        "3" => Gamble(),
        "4" => Exit(),
        _ => OptionHandleResult.InvalidOption,
    };
}

bool TryParsePositiveDouble( string input, out double value )
{
    if ( !double.TryParse( input, out value ) )
    {
        return false;
    }

    if ( value <= 0 || double.IsInfinity( value ) || double.IsNaN( value ) )
    {
        return false;
    }

    return true;
}

OptionHandleResult MakeDeposit()
{
    Console.Write( "Enter your deposit: " );
    string depositStr = Console.ReadLine();
    Console.WriteLine();

    if ( !TryParsePositiveDouble( depositStr, out double deposit )
        || double.MaxValue - deposit < balance )
    {
        return OptionHandleResult.InvalidDepositValue;
    }

    balance += deposit;

    return OptionHandleResult.Success;
}

OptionHandleResult ShowBalance()
{
    Console.WriteLine( $"Current balance: {balance}" );
    Console.WriteLine();
    return OptionHandleResult.Success;
}

OptionHandleResult Gamble()
{
    Console.Write( "Enter your bet: " );
    string betStr = Console.ReadLine();
    Console.WriteLine();

    if ( !TryParsePositiveDouble( betStr, out double bet )
        || bet > balance )
    {
        return OptionHandleResult.InvalidBetValue;
    }
    balance -= bet;

    int seed = Random.Shared.Next( 1, 21 );
    Console.WriteLine( $"Rolled number: {seed}" );

    if ( seed >= 18 && seed <= 20 )
    {
        double winAmount = CalculateWinAmount( bet, seed );

        if ( double.IsInfinity( winAmount )
            || double.IsNaN( winAmount )
            || double.MaxValue - winAmount < balance )
        {
            balance += bet;
            return OptionHandleResult.InvalidBetValue;
        }

        balance += winAmount;
        Console.WriteLine( $"You won {winAmount} credits!" );
    }
    else
    {
        Console.WriteLine( $"You lost {bet} credits." );
    }
    Console.WriteLine( $"Now your balance is {balance}." );
    Console.WriteLine();
    return OptionHandleResult.Success;
}

double CalculateWinAmount( double bet, int seed )
{
    const int multiplicator = 2;
    return bet * ( 1 + ( multiplicator * seed % 17 ) );
}

OptionHandleResult Exit()
{
    isGameFinished = true;
    Console.WriteLine( "See you later!" );
    return OptionHandleResult.Success;
}

enum OptionHandleResult
{
    Success = 0,
    InvalidOption = 1,

    InvalidDepositValue = 2,
    InvalidBetValue = 3,
}