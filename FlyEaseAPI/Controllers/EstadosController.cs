﻿using FlyEase_ApiRest_.Abstracts_and_Interfaces;
using FlyEase_ApiRest_.Models;
using FlyEase_ApiRest_.Models.Contexto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlyEase_ApiRest_.Controllers;

[EnableCors("Reglas")]
/// <summary>
/// Controlador de lectura para los Estados registrados.
/// </summary>
[SwaggerTag("Metodos de lectura para Estados")]
public class EstadosController : ReadController<Estado, int, FlyEaseDataBaseContextAuthentication>
{
    /// <summary>
    ///     Constructor del controlador de estados.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    /// <param name="hubContext">Contexto del hub.</param>
    public EstadosController(FlyEaseDataBaseContextAuthentication context) : base(context)
    {
        _context = context;
    }

    // Documentación de métodos heredados de la clase abstracta

    /// <summary>
    ///     Obtiene todos los elementos heredados de la clase base.
    /// </summary>
    [HttpGet("GetAll")]
    [Authorize(Policy = "Admin Policy")]
    [SwaggerOperation(Summary = "Obtener todos los Estados.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Consulta realizada con exito", typeof(List<Estado>))]
    public override async Task<IActionResult> Get()
    {
        var func = await base.Get();
        return func;
    }

    /// <summary>
    ///     Obtiene un elemento por ID heredado de la clase base.
    /// </summary>
    /// <param name="id">ID del elemento a obtener.</param>
    /// <returns>El elemento solicitado.</returns>
    [HttpGet("GetById/{id}")]
    [Authorize(Policy = "Admin Policy")]
    [SwaggerOperation(Summary = "Obtener un Estado por su ID.")]
    [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(Estado))]
    public override async Task<IActionResult> GetById(int id)
    {
        var func = await base.GetById(id);
        return func;
    }
}