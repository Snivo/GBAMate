namespace GBAMate.Core;

public struct Registers
{
    public readonly int[] Raw;

    public const int Status = 16;
    public const int StatusBank = 17;
    
    private const int FiqOffset = 17;
    private const int FiqStatus = 24;
    private const int SvcOffset = 25;
    private const int SvcStatus = 27;
    private const int AbtOffset = 28;
    private const int AbtStatus = 30;
    private const int IrqOffset = 31;
    private const int IrqStatus = 33;
    private const int UndOffset = 34;
    private const int UndStatus = 36;

    public int GetRawIndex(OperatingMode o, int index)
    {
        return o switch
        {
            OperatingMode.User or OperatingMode.System => index switch
            {
                >= 0 and < 17 => index,
                _ => throw new IndexOutOfRangeException()
            },
            
            OperatingMode.Fiq => index switch
            {
                >= 0 and < 8 or 15 or Status => index,
                >= 8 and < 15 => index - 8 + FiqOffset,
                17 => FiqStatus,
                _ => throw new IndexOutOfRangeException()
            },
            
            OperatingMode.Supervisor => index switch
            {
                >= 0 and < 13 or 15 or Status => index,
                >= 13 and < 15 => index - 13 + SvcOffset,
                17 => SvcStatus,
                _=> throw new IndexOutOfRangeException()
            },
            
            OperatingMode.Abort => index switch
            {
                >= 0 and < 13 or 15 or Status => index,
                >= 13 and < 15 => index - 13 + AbtOffset,
                17 => AbtStatus,
                _ => throw new IndexOutOfRangeException()
            },
            
            OperatingMode.Irq => index switch
            {
                >= 0 and < 13 or 15 or Status => index,
                >= 13 and < 15 => index - 13 + IrqOffset,
                17 => IrqStatus,
                _ => throw new IndexOutOfRangeException()
            },
            
            OperatingMode.Undefined => index switch
            {
                >= 0 and < 13 or 15 or Status => index,
                >= 13 and < 15 => index - 13 + UndOffset,
                17 => UndStatus,
                _ => throw new IndexOutOfRangeException()
            },
            _ => throw new ArgumentOutOfRangeException(nameof(o), o, null)
        };
    }
    
    public int this[OperatingMode o, int index]
    {
        get => Raw[GetRawIndex(o, index)];

        set => Raw[GetRawIndex(o, index)] = value;
    }
}