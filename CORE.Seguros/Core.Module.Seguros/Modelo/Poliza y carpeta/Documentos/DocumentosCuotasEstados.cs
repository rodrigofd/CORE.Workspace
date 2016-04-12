namespace FDIT.Core.Seguros
{
    public enum DocumentoCuotaEstado
    {
        Error = 0,
        Saldada = 1,
        Deudor = 2,
        Moroso = 3,
        Cobro = 4,
        PendLiqCia = 5,
        PendLiqCiaPago = 6,
        Rendicion = 7,
        RendicionCobro = 8,
        PendLiqInt = 9,
        PendLiqIntPago = 10
    }
}