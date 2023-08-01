using Business.Interfaces;
using Business.Models;
using Business.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace Business.Services
{
    // Todos os métodos que alteram uma entidade no banco (Add,Update) vão ser utilizados através de um servico
    public abstract class BaseService
    {
        private readonly INotifier _notifier;

        protected BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected void Notify(string message)
        {
            // Propaga o erro até a camada de apresentacao
            _notifier.Handle(new Notification(message));
        }

        protected void Notify(ValidationResult validationResult) 
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        // TV -> Classe da Validacao Generica
        //TE -> Classe de Entidade Generica
        protected bool ExecuteValidation<TV, TE>(TV validate, TE entity) where TV: AbstractValidator<TE> where TE: Entity
        {
            var validator = validate.Validate(entity);
            if (validator.IsValid) return true;
            Notify(validator);
            return false;
        }
    }
}