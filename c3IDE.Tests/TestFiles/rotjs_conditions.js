"use strict";

{
  C3.Plugins.aaXe_RotJs.Cnds = {
    //triggers
    MapGenerated() {
      return true;
    },

    ArenaGenerated() {
      return true;
    },

    MazeGenerated() {
      return true;
    },

    CellularAutomataGenerated() {
      return true;
    },

    DungeonGenerated() {
      return true;
    },

    RogueDungeonGenerated() {
      return true;
    },

    BlobsGenerated() {
      return true;
    },

    FovGenerated() {
      return true;
    },

    AutoTileGenerated() {
      return true;
    },

    //loops
    IterateRooms() {
      const runtime = this._runtime;
      const eventSheetManager = runtime.GetEventSheetManager();
      const currentEvent = runtime.GetCurrentEvent();
      const solModifiers = currentEvent.GetSolModifiers();
      const eventStack = runtime.GetEventStack();

      const oldFrame = eventStack.GetCurrentStackFrame();
      const newFrame = eventStack.Push(currentEvent);

      var index = 0;
      for (const room of this.rooms) {
        this.curRoomIndex = index;
        //console.log(this.curRoomIndex);

        this.curRoom = room;
        //console.log(this.curRoom);

        eventSheetManager.PushCopySol(solModifiers);
        currentEvent.Retrigger(oldFrame, newFrame);
        eventSheetManager.PopSol(solModifiers);

        index++;
      }

      eventStack.Pop();
      return false;
    },

    IterateBlobs() {
      const runtime = this._runtime;
      const eventSheetManager = runtime.GetEventSheetManager();
      const currentEvent = runtime.GetCurrentEvent();
      const solModifiers = currentEvent.GetSolModifiers();
      const eventStack = runtime.GetEventStack();

      const oldFrame = eventStack.GetCurrentStackFrame();
      const newFrame = eventStack.Push(currentEvent);

      var index = 0;
      for (const blob of this.blobs) {
        this.curBlobIndex = index;
        //console.log(this.curRoomIndex);

        this.curBlob = blob;
        //console.log(this.curRoom);

        eventSheetManager.PushCopySol(solModifiers);
        currentEvent.Retrigger(oldFrame, newFrame);
        eventSheetManager.PopSol(solModifiers);

        index++;
      }

      eventStack.Pop();
      return false;
    },

    IterateFovCells() {
      const runtime = this._runtime;
      const eventSheetManager = runtime.GetEventSheetManager();
      const currentEvent = runtime.GetCurrentEvent();
      const solModifiers = currentEvent.GetSolModifiers();
      const eventStack = runtime.GetEventStack();

      const oldFrame = eventStack.GetCurrentStackFrame();
      const newFrame = eventStack.Push(currentEvent);

      var index = 0;
      for (const fovCell of this.fovMap) {
        this.curFovIndex = index;
        this.curFovCell = fovCell;

        eventSheetManager.PushCopySol(solModifiers);
        currentEvent.Retrigger(oldFrame, newFrame);
        eventSheetManager.PopSol(solModifiers);

        index++;
      }

      eventStack.Pop();
      return false;
    },

    IterateAutoTileCells() {
      // const runtime = this._runtime;
      // const eventSheetManager = runtime.GetEventSheetManager();
      // const currentEvent = runtime.GetCurrentEvent();
      // const solModifiers = currentEvent.GetSolModifiers();
      // const eventStack = runtime.GetEventStack();

      // const oldFrame = eventStack.GetCurrentStackFrame();
      // const newFrame = eventStack.Push(currentEvent);

      // var index = 0;
      // for (const fovCell of this.fovMap) {
      //   this.curFovIndex = index;
      //   this.curFovCell = fovCell;

      //   eventSheetManager.PushCopySol(solModifiers);
      //   currentEvent.Retrigger(oldFrame, newFrame);
      //   eventSheetManager.PopSol(solModifiers);

      //   index++;
      // }

      // eventStack.Pop();
      // return false;
    },

    IterateDoors() {
      const runtime = this._runtime;
      const eventSheetManager = runtime.GetEventSheetManager();
      const currentEvent = runtime.GetCurrentEvent();
      const solModifiers = currentEvent.GetSolModifiers();
      const eventStack = runtime.GetEventStack();

      const oldFrame = eventStack.GetCurrentStackFrame();
      const newFrame = eventStack.Push(currentEvent);

      var index = 0;
      for (const door of this.doors) {
        this.curDoorIndex = index;
        //console.log(this.curDoorIndex);

        this.curDoor = door;
        //console.log(this.curDoor);

        eventSheetManager.PushCopySol(solModifiers);
        currentEvent.Retrigger(oldFrame, newFrame);
        eventSheetManager.PopSol(solModifiers);

        index++;
      }

      eventStack.Pop();
      return false;
    },

    IterateAllCells() {
      const runtime = this._runtime;
      const eventSheetManager = runtime.GetEventSheetManager();
      const currentEvent = runtime.GetCurrentEvent();
      const solModifiers = currentEvent.GetSolModifiers();
      const eventStack = runtime.GetEventStack();

      const oldFrame = eventStack.GetCurrentStackFrame();
      const newFrame = eventStack.Push(currentEvent);

      var index = 0;
      var mapKeys = Object.keys(this.map);
      for (const key of mapKeys) {
        this.curCellIndex = index;
        //console.log(this.curCellIndex);

        this.curCell = this.map[key];
        //console.log(this.curCell);

        eventSheetManager.PushCopySol(solModifiers);
        currentEvent.Retrigger(oldFrame, newFrame);
        eventSheetManager.PopSol(solModifiers);

        index++;
      }

      eventStack.Pop();
      return false;
    },

    IterateAllFloorCells() {
      const runtime = this._runtime;
      const eventSheetManager = runtime.GetEventSheetManager();
      const currentEvent = runtime.GetCurrentEvent();
      const solModifiers = currentEvent.GetSolModifiers();
      const eventStack = runtime.GetEventStack();

      const oldFrame = eventStack.GetCurrentStackFrame();
      const newFrame = eventStack.Push(currentEvent);

      var index = 0;
      var mapValues = Object.values(this.map);
      var floorCells = mapValues.filter(c => c.value === 0);
      for (const cell of floorCells) {
        this.curCellIndex = index;
        //console.log(this.curCellIndex);

        this.curCell = cell;
        //console.log(this.curCell);

        eventSheetManager.PushCopySol(solModifiers);
        currentEvent.Retrigger(oldFrame, newFrame);
        eventSheetManager.PopSol(solModifiers);

        index++;
      }

      eventStack.Pop();
      return false;
    },

    IterateRoomCells(roomIndex) {
      //check for room existance
      if (this.rooms.length <= roomIndex) return false;
      var rm = this.rooms[roomIndex];

      const runtime = this._runtime;
      const eventSheetManager = runtime.GetEventSheetManager();
      const currentEvent = runtime.GetCurrentEvent();
      const solModifiers = currentEvent.GetSolModifiers();
      const eventStack = runtime.GetEventStack();

      const oldFrame = eventStack.GetCurrentStackFrame();
      const newFrame = eventStack.Push(currentEvent);

      var index = 0;
      for (const cell of rm.cells) {
        this.curCellIndex = index;
        //console.log(this.curDoorIndex);

        this.curCell = cell;
        //console.log(this.curDoor);

        eventSheetManager.PushCopySol(solModifiers);
        currentEvent.Retrigger(oldFrame, newFrame);
        eventSheetManager.PopSol(solModifiers);

        index++;
      }

      eventStack.Pop();
      return false;
    },

    IterateBlobCells(blobIndex) {
      //check for room existance
      if (this.blobs.length <= blobIndex) return false;
      var blob = this.blobs[blobIndex];
      const runtime = this._runtime;
      const eventSheetManager = runtime.GetEventSheetManager();
      const currentEvent = runtime.GetCurrentEvent();
      const solModifiers = currentEvent.GetSolModifiers();
      const eventStack = runtime.GetEventStack();

      const oldFrame = eventStack.GetCurrentStackFrame();
      const newFrame = eventStack.Push(currentEvent);

      var index = 0;
      for (const cell of blob) {
        this.curCellIndex = index;
        //console.log(this.curDoorIndex);

        this.curCell = cell;
        //console.log(this.curDoor);

        eventSheetManager.PushCopySol(solModifiers);
        currentEvent.Retrigger(oldFrame, newFrame);
        eventSheetManager.PopSol(solModifiers);

        index++;
      }

      eventStack.Pop();
      return false;
    },

    //cells
    CellIsFloor(x, y) {
      return this.map[x + "," + y].value === 0;
    },

    CellIsWall(x, y) {
      return this.map[x + "," + y].value === 1;
    },

    CellIsRoom(x, y) {
      return this.map[x + "," + y].type === "room";
    },

    CellIsCooridor(x, y) {
      return this.map[x + "," + y].type === "coor";
    },

    //current cells
    CurrentCellIsFloor() {
      return this.map[this.curCell.x + "," + this.curCell.y].value == 0;
    },

    CurrentCellIsWall() {
      return this.map[this.curCell.x + "," + this.curCell.y].value == 1;
    },

    CurrentCellIsRoom() {
      return this.map[this.curCell.x + "," + this.curCell.y].type === "room";
    },

    CurrentCellIsCooridor() {
      return this.map[this.curCell.x + "," + this.curCell.y].type === "coor";
    }
  };
}
