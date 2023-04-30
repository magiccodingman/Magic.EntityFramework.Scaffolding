using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Magic.EntityFramework.Scaffolding;

public partial class TemplateDbContext : DbContext
{
    public TemplateDbContext()
    {
    }


    public TemplateDbContext(DbContextOptions<TemplateDbContext> options)
        : base(options)
    {
    }

    /*  DO NOT REMOVE THIS COMMENT
     * Anything below this comment will be removed on scaffolding
     */
    #region ScaffoldingCode
    
    #endregion
    /*  DO NOT REMOVE THIS COMMENT
     * End scaffold DbSets above this comment
     */

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(""); // Write connection string here!



    /*  DO NOT REMOVE THIS COMMENT
     * Anything below this comment will be removed on scaffolding
     */
    #region EnvironmentEnum
   
    #endregion
    /*  DO NOT REMOVE THIS COMMENT
     * End EnvironmentEnum above this comment
     */


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*  DO NOT REMOVE THIS COMMENT
         * Anything below this comment will be removed on scaffolding
         */
        #region ModelBuilding
       
        #endregion
        /*  DO NOT REMOVE THIS COMMENT
         * End ModelBuilding above this comment
         */

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
