import { useEffect, useReducer, useRef, useState } from "react";
import type { GameDetail } from "../models/gamesdetail.model";
import type { Genre } from "../models/genre.model";

function GameListComponent() {
  const [genres, setGenres] = useState<Genre[]>();
  const [games, setGames] = useState<GameDetail[]>();
  const [game, setGame] = useState<GameDetail>();
  const [selectedGenre, setSelectedGenre] = useState<Genre>();
  const [actionName, setActionName] = useState("Add New Game");

  const [ignored, forceUpdate] = useReducer((x) => x + 1, 0);

  const dialogRef = useRef<HTMLDialogElement | null>(null);

  const onCloseDialog = () => {
    setGame(undefined);
    setSelectedGenre(undefined);
    setActionName("Add New Game");
    dialogRef.current?.close();
  };

  const onShowDialog = (actionName: string, gameId: number) => {
    const selectedGame = games?.find((g) => g.id === gameId);
    selectedGame
      ? setGame({
          ...selectedGame,
          releaseDate: selectedGame.releaseDate.toString().split("T")[0],
        })
      : null;
    setActionName(actionName);
    dialogRef.current?.showModal();
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    const formData = new FormData(event.currentTarget);
    formData.append("genreId", JSON.stringify(selectedGenre?.id));
    const formJson = Object.fromEntries((formData as any).entries());

    const response = await fetch(`${import.meta.env.VITE_API_URL}/game`, {
      method: formJson.id ? "PUT" : "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(formJson),
    });
    const data = await response.json();

    dialogRef.current?.close();
    forceUpdate();
  };

  const onDeleteGame = async (id: number) => {
    const response = await fetch(`${import.meta.env.VITE_API_URL}/game/${id}`, {
      method: "DELETE",
    });

    const data = await response.json();

    const filterGames = games?.filter((game) => game.id !== data.id);
    setGames(filterGames);

    forceUpdate();
  };

  useEffect(() => {
    const fetchGenres = async () => {
      const genreResponse = await fetch(
        `${import.meta.env.VITE_API_URL}/genre`
      );
      const genreData = await genreResponse.json();
      setGenres(genreData);
    };

    const fetchGames = async () => {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/game`);
      const data = await response.json();
      setGames(data);
    };

    fetchGenres();
    fetchGames();
  }, [ignored]);

  return (
    <>
      <div className="container">
        <dialog ref={dialogRef} style={{ width: "50%" }}>
          <form method="dialog" onSubmit={handleSubmit}>
            <div className="modal-header mb-3">
              <h1 className="modal-title fs-5" id="staticBackdropLabel">
                {actionName}
              </h1>
              <button
                type="button"
                className="btn-close"
                data-bs-dismiss="modal"
                aria-label="Close"
                onClick={onCloseDialog}
              />
            </div>
            <div className="modal-body">
              <div className="mb-3 row">
                <label htmlFor="title" className="col-sm-2 col-form-label">
                  Title:
                </label>
                <div className="col-sm-10">
                  <input
                    type="text"
                    id="title"
                    name="title"
                    className="form-control"
                    placeholder="Title Name"
                    value={game?.title}
                  />
                </div>
              </div>
              <div className="mb-3 row">
                <label htmlFor="genre" className="col-sm-2 col-form-label">
                  Genre:
                </label>
                <div className="col-sm-10">
                  <select
                    onChange={(e) => {
                      console.log(e.target.value);
                      const genre = genres?.find(
                        (x) => x.id === Number(e.target.value)
                      );
                      setSelectedGenre(genre);
                    }}
                    className="form-control"
                    aria-label="Select Genre"
                  >
                    {actionName === "Add New Game" ? (
                      <option>{"Select a Genre"}</option>
                    ) : (
                      <option>{game?.genre}</option>
                    )}
                    {genres
                      ? genres.map((genre) => {
                          return (
                            <option key={genre.id} value={genre.id}>
                              {genre.name}
                            </option>
                          );
                        })
                      : null}
                  </select>
                </div>
              </div>
              <div className="mb-3 row">
                <label htmlFor="price" className="col-sm-2 col-form-label">
                  Price:
                </label>
                <div
                  className="col-sm-10 input-group"
                  style={{ width: "auto" }}
                >
                  <span className="input-group-text">€</span>
                  <input
                    type="number"
                    data-decimal-scale="2"
                    step="0.01"
                    id="price"
                    name="price"
                    className="form-control currency"
                    placeholder="0.0"
                    value={game?.price}
                  />
                </div>
              </div>
              <div className="mb-3 row">
                <label
                  htmlFor="releaseDate"
                  className="col-sm-2 col-form-label"
                >
                  Release Date:
                </label>
                <div className="col-sm-10">
                  <input
                    type="date"
                    id="releaseDate"
                    name="releaseDate"
                    className="form-control"
                    placeholder="Release Date"
                    value={game?.releaseDate}
                  />
                </div>
              </div>
              <div className="mb-3 row">
                <label htmlFor="developer" className="col-sm-2 col-form-label">
                  Developer:
                </label>
                <div className="col-sm-10">
                  <input
                    type="text"
                    id="developer"
                    name="developer"
                    className="form-control"
                    placeholder="Developer"
                    value={game?.developer}
                  />
                </div>
              </div>
              <div className="mb-3 row">
                <label htmlFor="publisher" className="col-sm-2 col-form-label">
                  Publisher:
                </label>
                <div className="col-sm-10">
                  <input
                    type="text"
                    id="publisher"
                    name="publisher"
                    className="form-control"
                    placeholder="Publisher"
                    value={game?.publisher}
                  />
                </div>
              </div>
            </div>
            <div className="modal-footer">
              <button className="btn btn-primary" type="submit">
                Save changes
              </button>
              <button className="btn btn-secondary" onClick={onCloseDialog}>
                Close
              </button>
            </div>
          </form>
        </dialog>
        <h1>List of Games</h1>
        <button
          className="btn btn-primary mb-3"
          onClick={() => onShowDialog("Add New Game", 0)}
        >
          Add New Game
        </button>
        {games && games.length === 0 && (
          <div className="alert alert-warning" role="alert">
            No games available.
          </div>
        )}
        <table className="table table-striped">
          <thead>
            <tr>
              <th>Id</th>
              <th>Title</th>
              <th>Genre</th>
              <th>Price</th>
              <th>Release Date</th>
              <th>Developer</th>
              <th>Publisher</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {games &&
              games.map((game) => (
                <tr key={game.id}>
                  <th scope="row">{game.id}</th>
                  <td>{game.title}</td>
                  <td>{game.genre}</td>
                  <td>{game.price}</td>
                  <td>{game.releaseDate.toString().split("T")[0]}</td>
                  <td>{game.developer}</td>
                  <td>{game.publisher}</td>
                  <td>
                    <button
                      className="btn btn-info"
                      onClick={() => onShowDialog("Edit Game", game.id)}
                    >
                      Edit
                    </button>
                    <button
                      className="btn btn-danger"
                      onClick={() => onDeleteGame(game.id)}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      </div>
    </>
  );
}

export default GameListComponent;
