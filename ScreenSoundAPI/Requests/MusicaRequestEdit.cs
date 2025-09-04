using System.Collections.Generic;

namespace ScreenSound.API.Requests;

public record MusicaRequestEdit(int Id, string nome, int ArtistaId, int anoLancamento, ICollection<GeneroRequest> generos) : MusicaRequest(nome, ArtistaId, anoLancamento, generos);