using System;
using System.Collections.Generic;

namespace UniriojaREST.Entities
{
    public static class ConcesionariosContextExtensions
    {
        public static void EnsureSeedDataForContext(this ConcesionariosContext context)
        {
            context.Clientes.RemoveRange(context.Clientes);
            context.SaveChanges();
            
            var clientes = new List<Cliente>()
            {
                new Cliente()
                {
                     Id = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf"),
                     Nombre = "Pepito",
                     Apellido = "Grillo",
                     Telefono = "999999999",
                     EsVip = true,
                     Presupuestos = new List<Presupuesto>()
                     {
                         new Presupuesto()
                         {
                             Id = new Guid("c7ba6add-09c4-45f8-8dd0-eaca221e5d93"),
                             Estado = PresupuestoEstado.Abierto,
                             ImporteNegociado = 9500,
                             Observaciones = "Opel Astra 2015 120CV"
                        },
                        new Presupuesto()
                         {
                             Id = new Guid("0c670d91-948d-4860-88c0-16683a2a59c7"),
                             Estado = PresupuestoEstado.Aceptado,
                             ImporteNegociado = 400,
                             Observaciones = "Renault Megane 2004"
                        },
                        new Presupuesto()
                         {
                             Id = new Guid("97ca2d4c-2405-4f18-9b2b-3583a77a376f"),
                             Estado = PresupuestoEstado.Cerrado,
                             ImporteNegociado = 400,
                             Observaciones = "Renault Megane 2004"
                        }
                     }
                },
                new Cliente()
                {
                     Id = new Guid("6f63af95-d62a-4f5d-86e9-3ee12b0085d2"),
                     Nombre = "Mulán",
                     Apellido = "Fa",
                     Telefono = "54353454534",
                     EsVip = false,
                     Presupuestos = new List<Presupuesto>()
                },
                new Cliente()
                {
                     Id = Guid.NewGuid(),
                     Nombre = "Blanca",
                     Apellido = "Nieves",
                     Telefono = "123456789",
                     EsVip = false,
                     Presupuestos = new List<Presupuesto>()
                },
                new Cliente()
                {
                     Id = Guid.NewGuid(),
                     Nombre = "Bella",
                     Apellido = "Bestia",
                     Telefono = "123456789",
                     EsVip = false,
                     Presupuestos = new List<Presupuesto>()
                },
                new Cliente()
                {
                     Id = Guid.NewGuid(),
                     Nombre = "Anastasia",
                     Apellido = "Romanov",
                     Telefono = "123456789",
                     EsVip = false,
                     Presupuestos = new List<Presupuesto>()
                },
                new Cliente()
                {
                     Id = Guid.NewGuid(),
                     Nombre = "John",
                     Apellido = "Smith",
                     Telefono = "123456789",
                     EsVip = false,
                     Presupuestos = new List<Presupuesto>()
                },
                new Cliente()
                {
                     Id = Guid.NewGuid(),
                     Nombre = "Peter",
                     Apellido = "Pan",
                     Telefono = "123456789",
                     EsVip = false,
                     Presupuestos = new List<Presupuesto>()
                },
                new Cliente()
                {
                     Id = Guid.NewGuid(),
                     Nombre = "Tarzán",
                     Apellido = "Jane",
                     Telefono = "123456789",
                     EsVip = false,
                     Presupuestos = new List<Presupuesto>()
                },
                new Cliente()
                {
                     Id = Guid.NewGuid(),
                     Nombre = "Elsa",
                     Apellido = "Frozen",
                     Telefono = "123456789",
                     EsVip = false,
                     Presupuestos = new List<Presupuesto>()
                },
                new Cliente()
                {
                     Id = Guid.NewGuid(),
                     Nombre = "Ceni",
                     Apellido = "Cienta",
                     Telefono = "123456789",
                     EsVip = false,
                     Presupuestos = new List<Presupuesto>()
                }
            };

            context.Clientes.AddRange(clientes);
            context.SaveChanges();
        }
    }
}
