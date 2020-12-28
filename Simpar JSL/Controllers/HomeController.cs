using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simpar_JSL.Models;
using Simpar_JSL.Data;
using Simpar_JSL.Core;
using Newtonsoft.Json;

namespace Simpar_JSL.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            CadastroViewModels model = new CadastroViewModels();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CadastroViewModels model)
        {
            JsonResult result = null;
            List<Usuarios> usuarios = new List<Usuarios>();

            try
            {

                using (SimparlEntities db = new SimparlEntities())
                {

                    usuarios = db.Usuarios.ToList();
 
                    model.ListUsuarios = (from i in usuarios
                                          select new UsuariosViewModel
                                          {
                                              Id = i.Id,
                                              Nome = i.NomeCompleto != null ? i.NomeCompleto : "-",
                                              Marca = i.Marca != null ? i.Marca : "-",
                                              Modelo = i.Modelo != null ? i.Modelo : "-",
                                              Placa = i.Placa != null ? i.Placa : "-",
                                              Eixos = i.Eixos != null ? i.Eixos : 0,
                                              Rua = i.Rua != null ? i.Rua : "-",
                                              Cidade = i.Cidade != null ? i.Cidade : "-",
                                              Numero = i.Numero != null ? i.Numero : 0,
                                              Cep = i.Cep != null ? i.Cep : "-",
                                              Bairro = i.Bairro != null ? i.Bairro : "-",
                                              Estado = i.Estado != null ? i.Estado : "-",
                                              Observacoes = i.Observacoes,
                                              StatusCaminhao = i.StatusCaminhao,
                                              StatusMotorista = i.StatusMorotista,
                                              DataCadastro = i.DataCriacao,
                                              DataAlteracao = i.DataAlteracao

                                          }).OrderByDescending(p => p.DataCadastro).ToList();
                }

                result = new JsonResult() { Data = new JsonObject(true, "", "Data", JsonConvert.SerializeObject(model)) };
            }
            catch (Exception ex)
            {
                result = new JsonResult() { Data = new JsonObject(false, ex.Message, "Data", null) };
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult Save(CadastroViewModels model)
        {
            JsonResult result = null;

            try
            {
                using (SimparlEntities db = new SimparlEntities())
                {
                    if (model.Id.HasValue)
                    {

                        Usuarios usuarios = new Usuarios();

                        usuarios = db.Usuarios.Find(model.Id);

                        if (usuarios == null)
                            throw new Exception("Falha ao Consultar os dados do usuário!");

                        usuarios.NomeCompleto = model.Nome;
                        usuarios.Marca = model.Marca;
                        usuarios.Modelo = model.Modelo;
                        usuarios.Placa = model.Placa;
                        usuarios.Eixos = model.Eixos;
                        usuarios.Rua = model.Rua;
                        usuarios.Cidade = model.Cidade;
                        usuarios.Numero = model.Numero;
                        usuarios.Cep = model.Cep;
                        usuarios.Bairro = model.Bairro;
                        usuarios.Estado = model.Estado;
                        usuarios.Observacoes = model.Observacoes;
                        usuarios.StatusCaminhao = model.StatusCaminhao;
                        usuarios.StatusMorotista = model.StatusMotorista;
                        usuarios.DataAlteracao = DateTime.Now;

                        db.SaveChanges();

                        result = new JsonResult() { Data = new JsonObject(true, "Usuario alterado com sucesso", "Data", null) };
                    }
                    else
                    {
                        Usuarios usuarios = new Usuarios()
                        {
                            NomeCompleto = model.Nome,
                            Marca = model.Marca,
                            Modelo = model.Modelo,
                            Placa = model.Placa,
                            Eixos = model.Eixos,
                            Rua = model.Rua,
                            Cidade = model.Cidade,
                            Numero = model.Numero,
                            Cep = model.Cep,
                            Bairro = model.Bairro,
                            Estado = model.Estado,
                            Observacoes = model.Observacoes,
                            StatusCaminhao = model.StatusCaminhao,
                            StatusMorotista = model.StatusMotorista,
                            DataCriacao = DateTime.Now,
                            DataAlteracao = DateTime.Now
                        };

                        db.Usuarios.Add(usuarios);
                        db.SaveChanges();

                        result = new JsonResult() { Data = new JsonObject(true, "Usuario cadastrado com sucesso", "Data", null) };
                    }

                }

            }
            catch (Exception ex)
            {
                result = new JsonResult() { Data = new JsonObject(false, ex.Message, "Data", null) };
            }

            return result;
        }

        [HttpPost]
        public JsonResult Editar(int Id)
        {

            CadastroViewModels model = new CadastroViewModels();
            JsonResult json = null;

            try
            {
                Usuarios usuarios = new Usuarios();

                using (SimparlEntities db = new SimparlEntities())
                {
                    usuarios = db.Usuarios.Where(p => p.Id == Id).FirstOrDefault();
                }

                if (usuarios == null)
                    throw new Exception("Nenhum usuário encontrado para esse Id");

                model.Id = usuarios.Id;
                model.Nome = usuarios.NomeCompleto;
                model.Marca = usuarios.Marca;
                model.Modelo = usuarios.Modelo;
                model.Placa = usuarios.Placa;
                model.Rua = usuarios.Rua;
                model.Cidade = usuarios.Cidade;
                model.Cep = usuarios.Cep;
                model.Bairro = usuarios.Bairro;
                model.Estado = usuarios.Estado;
                model.Observacoes = usuarios.Observacoes;
                model.Eixos = usuarios.Eixos;
                model.Numero = usuarios.Numero;
                model.StatusMotorista = usuarios.StatusMorotista;
                model.StatusCaminhao = usuarios.StatusCaminhao;

                json = new JsonResult() { Data = new JsonObject(true, null, "Data", JsonConvert.SerializeObject(model)) };

            }
            catch (Exception ex)
            {
                json = new JsonResult() { Data = new JsonObject(false, string.Format($"Erro ao consultar os dados do negócio Id:{Id}", ex.Message), "Data", null) };
            }

            return json;
        }

        [HttpPost]
        public ActionResult Delete(int IdUsuario)
        {
            JsonResult result = null;

            if (ModelState.IsValid)
            {
                Usuarios usuario = new Usuarios();

                if (string.IsNullOrEmpty(IdUsuario.ToString()))
                    throw new Exception("Falha ao deletar usuário!");

                try
                {
                    using (SimparlEntities db = new SimparlEntities())
                    {
                        usuario = db.Usuarios.Find(IdUsuario);

                        db.Usuarios.Remove(usuario);
                        db.SaveChanges();
                    }

                    result = new JsonResult() { Data = new JsonObject(true, "Usuario deletado com sucesso", "Data", null) };
                }
                catch (Exception ex)
                {
                    result = new JsonResult() { Data = new JsonObject(false, ex.Message, "Data", null) };
                }

            }

            return result;
        }
    }
}