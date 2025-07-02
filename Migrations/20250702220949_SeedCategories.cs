using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetPharmacyApi.Migrations
{
    public partial class SeedCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Parasite Control" },
                    { 2, "Pain Relief" },
                    { 3, "Vitamins and Supplements" },
                    { 4, "Antibiotics" },
                    { 5, "Dental Care" },
                    { 6, "Skin and Coat Care" },
                    { 7, "Digestive Health" },
                    { 8, "Eye and Ear Care" },
                    { 9, "Vaccines" },
                    { 10, "Heartworm Prevention" },
                    { 11, "Flea and Tick Control" },
                    { 12, "Behavioral Health" },
                    { 13, "Nutritional Support" },
                    { 14, "Surgical Supplies" },
                    { 15, "Anesthesia" },
                    { 16, "Diagnostic Tools" },
                    { 17, "Hospital Equipment" },
                    { 18, "Wound Care" },
                    { 19, "Reproductive Health" },
                    { 20, "Joint Health" },
                    { 21, "Cold and Flu" },
                    { 22, "Grooming Supplies" },
                    { 23, "Training Aids" },
                    { 24, "Toys and Enrichment" },
                    { 25, "Food and Diet" },
                    { 26, "Supplements for Cats" },
                    { 27, "Supplements for Dogs" },
                    { 28, "Supplements for Birds" },
                    { 29, "Supplements for Small Mammals" },
                    { 30, "Supplements for Reptiles" },
                    { 31, "Infectious Disease Control" },
                    { 32, "Parasite Testing" },
                    { 33, "First Aid" },
                    { 34, "Emergency Care" },
                    { 35, "Hydration Therapy" },
                    { 36, "Eye Drops" },
                    { 37, "Ear Drops" },
                    { 38, "Anti-inflammatory" },
                    { 39, "Steroids" },
                    { 40, "Anti-fungal" },
                    { 41, "Anti-viral" },
                    { 42, "Pain Management" },
                    { 43, "Cold Packs" },
                    { 44, "Hot Packs" },
                    { 45, "Bandages" },
                    { 46, "Splints and Casts" },
                    { 47, "Hospital Apparel" },
                    { 48, "Cleaning Supplies" },
                    { 49, "Disinfectants" },
                    { 50, "Miscellaneous" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i <= 50; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Categories",
                    keyColumn: "Id",
                    keyValue: i);
            }
        }
    }
}
