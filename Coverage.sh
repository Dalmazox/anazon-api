echo "Coletando dados"
dotnet test --collect:"XPlat Code Coverage"

echo "Gerando relatório"
reportgenerator "-reports:**\coverage.cobertura.xml" "-targetdir:coverage-report" -reporttypes:Html