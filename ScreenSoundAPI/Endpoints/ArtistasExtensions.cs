using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class ArtistasExtensions
    {
        private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
        {
            return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
        }

        private static ArtistaResponse EntityToResponse(Artista artista)
        {
            return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
        }

        public static void AddEndPointsArtistas(this WebApplication app)
        {
            app.MapGet("/artistas", ([FromServices] DAL<Artista> dal) => {
                return Results.Ok(dal.Listar());
            });

            app.MapGet("/artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) => {
                var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

                if (artista is null)
                    return Results.NotFound();

                return Results.Ok(artista);
            });

            app.MapPost("/artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequest artistaRequest) =>
            {
                var artista = new Artista(artistaRequest.nome, artistaRequest.bio);
                dal.Adicionar(artista);
                return Results.Ok();
            });

            app.MapDelete("/artistas/{id}", ([FromServices] DAL<Artista> dal, int id) =>
            {
                var artista = dal.RecuperarPor(a => a.Id.Equals(id));

                if (artista is null)
                    return Results.NotFound();

                dal.Deletar(artista);
                return Results.NoContent();
            });

            app.MapPut("/artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequestEdit artistaRequestEdit) =>
            {
                var artistaAtualizar = dal.RecuperarPor(a => a.Id.Equals(artistaRequestEdit.Id));

                if (artistaAtualizar is null)
                    return Results.NotFound();

                artistaAtualizar.Nome = artistaRequestEdit.nome;
                artistaAtualizar.Bio = artistaRequestEdit.bio;
                dal.Atualizar(artistaAtualizar);

                return Results.Ok();
            });
        }
    }
}
