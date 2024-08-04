using SISDEN.Models;
using SISDEN.DTOS;
using System.Runtime.InteropServices;

namespace SISDEN.Services
{
    public interface IVerificarCedula
    {

        Task<bool> VerificarCedula(string cedula);  
    }

    public class VerificarCedulaServices: IVerificarCedula
    {
        private readonly SisdemContext _context;
        
        public VerificarCedulaServices(SisdemContext context) { 
            _context = context; 
        }

        public Task<bool> VerificarCedula(string cedula)
        {

           var verificarCedula = new VerificarCedulaDTO();
           
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0 || i == 0)
                {
                verificarCedula.multiplo.Add("1");
                }
                else
                {
                verificarCedula.multiplo.Add("2");
                }
            }
                string cedulaSinGuion  = "";

                    foreach (char c in cedula)
                    {
                        if (c >= '0' && c <= '9' && c != '-')
                        {
                            cedulaSinGuion = ((string.Concat(cedulaSinGuion, c)));
                        }
                    }
                
                verificarCedula.suma = 0;
                verificarCedula.longitud = verificarCedula.multiplo.Count;
                    foreach (char c in cedulaSinGuion)
                    {
                        if (cedulaSinGuion != "" && verificarCedula.word <= 9)
                        {
                        verificarCedula.producto = Convert.ToInt16(c - 48) * Convert.ToInt16(verificarCedula.multiplo[verificarCedula.word]);
                        verificarCedula.suma += (verificarCedula.producto / 10) + (verificarCedula.producto % 10);
                        }
                    verificarCedula.word++;
                    }
                    verificarCedula.entero = (verificarCedula.suma / 10) * 10;
                    if (verificarCedula.entero < verificarCedula.suma)
                    {
                        verificarCedula.entero += 10;
                    }
                    verificarCedula.digVerificador = Convert.ToString(verificarCedula.entero - verificarCedula.suma);
                    if (verificarCedula.digVerificador == cedula.Substring(12, 1))
                    {
                    verificarCedula.result = true;
                    }
                    else
                    {
                    verificarCedula.result = (false);
                    }
                verificarCedula.word = 0;


            
            return Task.FromResult(verificarCedula.result);


        }
    }

}
