namespace Task_1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfirstdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        StudentName = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                        Address = c.String(),
                        MobileNumber = c.String(),
                        CountryId = c.Int(nullable: false),
                        GradeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.Grades", t => t.GradeId, cascadeDelete: true)
                .Index(t => t.CountryId)
                .Index(t => t.GradeId);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        GradeId = c.Int(nullable: false, identity: true),
                        GradeName = c.String(),
                    })
                .PrimaryKey(t => t.GradeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "GradeId", "dbo.Grades");
            DropForeignKey("dbo.Students", "CountryId", "dbo.Countries");
            DropIndex("dbo.Students", new[] { "GradeId" });
            DropIndex("dbo.Students", new[] { "CountryId" });
            DropTable("dbo.Grades");
            DropTable("dbo.Students");
            DropTable("dbo.Countries");
        }
    }
}
