using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Simpar_JSL.Models
{
    public class CadastroViewModels
    {
        public int? Id { get; set; }
        public int? IdUsuario { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string Nome { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string Marca { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string Modelo { get; set; }

        [Display(Name = "Placa")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string Placa { get; set; }

        [Display(Name = "Eixos")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public int? Eixos { get; set; }

        [Display(Name = "Rua/Av")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string Rua { get; set; }

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string Cidade { get; set; }

        [Display(Name = "Numero")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public int? Numero { get; set; }

        [Display(Name = "Cep")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string Cep { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string Bairro { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        public string Estado { get; set; }

        [Display(Name = "Observacoes")]
        public string Observacoes { get; set; }

        [Display(Name = "Ativo")]
        public bool StatusMotorista { get; set; }

        [Display(Name = "Ativo")]
        public bool StatusCaminhao { get; set; }

        public List<UsuariosViewModel> ListUsuarios { get; set; }

        public CadastroViewModels()
        {
            this.ListUsuarios = new List<UsuariosViewModel>();
            this.StatusCaminhao = true;
            this.StatusMotorista = true;
        }
    }

    public class UsuariosViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        public int? Eixos { get; set; }
        public string Rua { get; set; }
        public string Cidade { get; set; }
        public int? Numero { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Observacoes { get; set; }
        public bool? StatusCaminhao { get; set; }
        public bool? StatusMotorista { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}