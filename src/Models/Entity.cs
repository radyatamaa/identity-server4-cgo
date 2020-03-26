using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Models
{

    public abstract class Entity : IDisposable
    {
        private int? _requestedHashCode;

        public Entity(Guid id)
        {
            Id = id;
        }

        public DateTimeOffset CreatedDate { get; set; }

        [Required]
        [MaxLength(64)]
        public string CreatedBy { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        [MaxLength(64)]
        public string ModifiedBy { get; set; }

        [Column("IsActive", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsActive { get; set; }

        [Key]
        public Guid Id { get; protected set; }

        #region Comparison
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            //if (item.IsTransient() || this.IsTransient())
            //    return false;
            //else
            return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)
                _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

            return _requestedHashCode.Value;
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
        #endregion

        #region Disposable
        // Flag: Has Dispose already been called?
        bool _disposed = false;

        // Instantiate a SafeHandle instance.
        System.Runtime.InteropServices.SafeHandle handle = new Microsoft.Win32.SafeHandles.SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                handle.Dispose();
            }

            // Free any unmanaged objects here.
            _disposed = true;
        }
        #endregion
    }

    public abstract class EntitySoftDelete : Entity
    {
        public EntitySoftDelete(Guid id) : base(id)
        {
        }

        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        [MaxLength(64)]
        public string DeleteBy { get; set; }
    }

}
