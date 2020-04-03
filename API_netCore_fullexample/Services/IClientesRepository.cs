using System;
using System.Collections.Generic;
using UniriojaREST.Entities;
using UniriojaREST.Helpers;
using UniriojaREST.Helpers.Filters;

namespace UniriojaREST.Services
{
    public interface IClientesRepository
    {
        PagedList<Cliente> GetClientes(ClientesFilter clientesFilter);
        
        Cliente GetCliente(Guid clienteId);

        void AddCliente(Cliente cliente);

        void DeleteCliente(Cliente cliente);
        
        void UpdateCliente(Cliente cliente);

        bool ClienteExists(Guid clienteId);

        IEnumerable<Presupuesto> GetPresupuestosForCliente(Guid clienteId);
        
        bool Save();
    }
}