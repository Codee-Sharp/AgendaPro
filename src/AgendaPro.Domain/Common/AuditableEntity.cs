namespace AgendaPro.Domain.Common
{
    /// 🧩 Classe base genérica para entidades auditáveis.
    /// ---------------------------------------------------
    /// Esta classe serve como modelo para todas as entidades do domínio
    /// que precisam registrar informações de auditoria.
    /// 📘 O que ela faz:
    /// - Armazena quem criou, atualizou ou excluiu um registro;
    /// - Guarda as datas dessas ações;
    /// - Controla exclusão lógica (soft delete) por meio da propriedade IsDeleted;
    /// - Evita duplicação de código entre entidades (User, Project, etc.).
    /// 📦 Exemplo:
    ///   public class User : AuditableEntity<Guid>
    ///   {
    ///       public string Name { get; private set; }
    ///       public string Email { get; private set; }
    ///
    ///       public User(string name, string email, string createdBy)
    ///       {
    ///           Id = Guid.NewGuid();
    ///           Name = name;
    ///           Email = email;
    ///           MarkCreated(DateTimeOffset.UtcNow, createdBy);
    ///       }
    ///   }
    /// 💡 Uso comum em aplicações com DDD e Clean Architecture.
    public abstract class AuditableEntity<TId>
    {
        public TId Id { get; protected set; }
    
        public DateTimeOffset? CreatedAt { get; protected set; }
        public string? CreatedBy { get; protected set; }
    
        public DateTimeOffset? UpdetadAt { get; protected set; }
        public string? UpdatedBy { get; protected set; }
    
        public DateTimeOffset? DeletedAt { get; protected set; }
        public string? DeletedBy { get; protected set; }
    
        public bool IsDeleted { get; protected set; }

        public void MarkCreated(DateTimeOffset now, string? user)
        {
            CreatedAt = now;
            UpdatedBy = user;
            IsDeleted = false;
        }

        public void MarkUpdated(DateTimeOffset now, string? User)
        {
            UpdetadAt = now;
            UpdatedBy = User;
        
        }

        public void SoftDelete(DateTimeOffset now, string? User)
        {
            IsDeleted = true;
            DeletedAt = now;
            DeletedBy = User;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }
    
    }
}