using System;
using System.Linq;
using c3IDE.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace c3IDE.Tests
{
    [TestClass]
    public class JavascriptManagerTest
    {
        [TestMethod]
        public void TestActionParseHappyPath()
        {
            var code = @"C3.Plugins.aaXe_Log.Acts =
	{
		writeLog (log, type) 
		{

			if (!this._loggingActive) return;
			console.log("" % c"" + this.iconTypes[type] + "" "" + log, this.logTypes[type]);


        }
    };";

            var funcs = JavascriptManager.GetAllFunction(code);
            Assert.AreEqual(funcs.Count, 1);
            Assert.AreEqual(funcs.FirstOrDefault().Key, "writeLog");
        }

        [TestMethod]
        public void TestActionParseComment()
        {
            var code = @"C3.Plugins.aaXe_Log.Acts =
	{
		writeLog (log, type) //test
		{

			if (!this._loggingActive) return;
			console.log("" % c"" + this.iconTypes[type] + "" "" + log, this.logTypes[type]);


        }
    };";

            var funcs = JavascriptManager.GetAllFunction(code);
            Assert.AreEqual(funcs.Count, 1);
            Assert.AreEqual(funcs.FirstOrDefault().Key, "writeLog");
        }

        [TestMethod]
        public void TestActionParseLongComment()
        {
            var code = @"C3.Plugins.aaXe_Log.Acts =
	{
		writeLog (log, type) /* test */
		{

			if (!this._loggingActive) return;
			console.log("" % c"" + this.iconTypes[type] + "" "" + log, this.logTypes[type]);


        }
    };";

            var funcs = JavascriptManager.GetAllFunction(code);
            Assert.AreEqual(funcs.Count, 1);
            Assert.AreEqual(funcs.FirstOrDefault().Key, "writeLog");
        }

        [TestMethod]
        public void TestActionParseHappyPathComplex()
        {
            var code = @"C3.Plugins.aaXe_Log.Acts =
	{
		GenerateArenaToTileMap(width, height, tm) {
			this._clearRooms();
			this.width = width;
			this.height = height;
			var instance = tm.GetInstances()[0].GetSdkInstance();
			this.gen = new ROT.Map.Arena(width, height);
			this.map = {};

			this.gen.create((x, y, value) => {
				var key = x + "","" + y;

            this.map[key] = {
                x: x,
                y: y,
                value: value,
                type: value === 1 ? 'wall' : 'floor'

            };

            //populate tilemap
            value? instance.SetTileAt(x, y, this._getWallTile()) : instance.SetTileAt(x, y, this._getFloorTile());
        });

        this.Trigger(C3.Plugins.aaXe_RotJs.Cnds.MapGenerated);
            this.Trigger(C3.Plugins.aaXe_RotJs.Cnds.ArenaGenerated);
    }
};";

            var funcs = JavascriptManager.GetAllFunction(code);
            Assert.AreEqual(funcs.Count, 1);
            Assert.AreEqual(funcs.FirstOrDefault().Key, "GenerateArenaToTileMap");
        }

        [TestMethod]
        public void TestCompleteFile()
        {
            var code = System.IO.File.ReadAllText("TestFiles\\rotjs_conditions.js");
            var funcs = JavascriptManager.GetAllFunction(code);
            Assert.AreEqual(funcs.Count, 26);
        }

        [TestMethod]
        public void TestNested()
        {
            var code = @"IterateRooms() {
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
    },";

            var funcs = JavascriptManager.GetAllFunction(code);
            Assert.AreEqual(funcs.Count, 1);
        }

        [TestMethod]
        public void TestNestedMultiple()
        {
            var code = @"IterateRooms() {
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
IterateRooms2() {
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
    },";

            var funcs = JavascriptManager.GetAllFunction(code);
            Assert.AreEqual(funcs.Count, 2);
        }

        [TestMethod]
        public void QuickFunctionTest()
        {
            var code = @"""use strict"";
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
    } 
  }
}";
            var funcs = JavascriptManager.GetAllFunction(code);
            Assert.AreEqual(funcs.Count, 3);
        }
    }


}
