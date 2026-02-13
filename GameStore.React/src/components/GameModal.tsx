import React from "react";

const GameModal = (open: boolean) => {
  if (!open) return false;
  return (
    <>
      <div
        className="modal fade"
        id="staticBackdrop"
        data-bs-backdrop="static"
        data-bs-keyboard="false"
        aria-labelledby="staticBackdropLabel"
        aria-hidden="true"
      >
        <form method="dialog">
          <div className="modal-header">
            <h1 className="modal-title fs-5" id="staticBackdropLabel">
              Modal title
            </h1>
            <button
              type="button"
              className="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div className="modal-body"></div>
          <div className="modal-footer">
            <button className="btn btn-primary">Save changes</button>
            <button className="btn btn-secondary">Close</button>
          </div>
        </form>
      </div>
    </>
  );
};
export default GameModal;
