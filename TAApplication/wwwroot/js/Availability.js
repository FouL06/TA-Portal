/**
* Author:    Ashton Foulger
* Partner:   Kyle Charlton
* Date:      11/22/22
* Course:    CS 4540, University of Utah, School of Computing
* Copyright: CS 4540 and Ashton Foulger - This work may not be copied for use in Academic Coursework.
*
* I, Ashton Foulger and Kyle Charlton, certify that I wrote this code from scratch and did 
* not copy it in part or whole from another source.  Any references used 
* in the completion of the assignment are cited in my README file and in
* the appropriate method header.
*
* File Contents:
*  Availablilty JS Code
*/

"use strict";

let _mousePressed = false;

class TimeSlot extends PIXI.Graphics {

    constructor(x, y, width, height, isInteractive) {
        super();

        this.interactive = isInteractive;
        this.Available = false;
        this.x = x;
        this.y = y;
        this.availableColor = 0xFEDCBA;
        this.unavailableColor = 0x567899;
        this.onchange = null;

        this.slotWidth = width;
        this.slotHeight = height;

        this.on('mousedown', this.mousedown);
        this.on('mouseover', this.mouseover);
        this.on('mouseup', this.mouseup);

        this.drawSelf();
    }

    mouseup(event) {
        _mousePressed = false;
    }

    mousedown(event) {
        _mousePressed = true;
        this.toggleAvailability();
    }

    mouseover(event) {
        this.toggleAvailability();
    }

    toggleAvailability() {
        if (_mousePressed) {
            this.setAvailability(!this.Available);
        }
    }

    setAvailability(available) {
        this.Available = available;
        this.drawSelf();

        if (this.onchange)
            this.onchange();
    }

    drawSelf() {
        this.beginFill(this.Available ? this.availableColor : this.unavailableColor);
        this.drawRect(0, 0, this.slotWidth, this.slotHeight);
        this.endFill();
    }
}

class AvailabilityTracker extends PIXI.Container {
    constructor(x, y, slotWidth, slotHeight, slotMargin, isInteractive) {
        super();

        this.dirty = false;

        this.x = x;
        this.y = y;

        const textStyle = new PIXI.TextStyle({
            fontFamily: 'Arial',
            fontSize: 16,
            fill: ['#ffffff']
        });

        this.weekdayText = [
            new PIXI.Text('Monday', textStyle),
            new PIXI.Text('Tuesday', textStyle),
            new PIXI.Text('Wednesday', textStyle),
            new PIXI.Text('Thursday', textStyle),
            new PIXI.Text('Friday', textStyle)
        ];

        for (let i = 0; i < 5; i++) {
            this.weekdayText[i].x = 5 + slotMargin + (i * (slotWidth + slotMargin));
            this.addChild(this.weekdayText[i]);
        }

        this.hourText = [];
        let hours = [8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8];
        for (let i = 0; i < hours.length; i++) {
            let text = `${hours[i]}:00 `;
            text += (i < 12) ? 'am' : 'pm';

            let pixiText = new PIXI.Text(text, textStyle);
            pixiText.x = (5 * (slotWidth + slotMargin)) + slotMargin + 20;
            pixiText.y = (4 * i * slotHeight) + 22;
            this.hourText.push(pixiText);
            this.addChild(pixiText);
        }


        this.TimeSlotContainer = new PIXI.Container();
        this.TimeSlotContainer.sortableChildren = true;
        this.TimeSlotContainer.y = 35;

        this.TimeSlots = Array.from(Array(5), () => new Array(48));
        for (let i = 0; i < 5; i++) {
            for (let j = 0; j < 48; j++) {
                let slot = new TimeSlot(i * (slotWidth + slotMargin) + slotMargin, j * slotHeight, slotWidth, slotHeight, isInteractive);
                slot.onchange = this.onDirty.bind(this);
                slot.zIndex = 0;
                this.TimeSlots[i][j] = slot;
                this.TimeSlotContainer.addChild(slot);
            }
        }

        for (let i = 0; i < (48 / 4) + 1; i++) {
            let line = new PIXI.Graphics();

            line.lineStyle(1, 0xffffff, 1);
            line.moveTo(0, 4 * i * slotHeight);
            line.lineTo(5 * (slotWidth + slotMargin) + slotMargin, 4 * i * slotHeight);
            line.zIndex = 1;
            this.TimeSlotContainer.addChild(line);
        }

        this.addChild(this.TimeSlotContainer);


        this.DescriptionContainer = new PIXI.Container();
        this.DescriptionContainer.x = slotMargin;
        this.DescriptionContainer.y = (slotHeight * 48) + 60;

        const descriptionStyle = new PIXI.TextStyle({
            fontFamily: 'Arial',
            fontSize: 14,
            fill: ['#ffffff']
        });

        this.descriptionText = new PIXI.Text('Click and drag to set/un-set available times.', descriptionStyle);
        this.DescriptionContainer.addChild(this.descriptionText);

        this.exampleAvailable = new TimeSlot(300, 6, slotWidth, slotHeight, false);
        this.exampleAvailable.setAvailability(true);
        this.DescriptionContainer.addChild(this.exampleAvailable);
        this.availableText = new PIXI.Text('Available', textStyle);
        this.availableText.x = 318;
        this.availableText.y = 8 + slotHeight;
        this.DescriptionContainer.addChild(this.availableText);

        this.exampleUnavailable = new TimeSlot(300 + (1.5 * slotWidth), 6, slotWidth, slotHeight, false);
        this.exampleUnavailable.setAvailability(false);
        this.DescriptionContainer.addChild(this.exampleUnavailable);
        this.unavailableText = new PIXI.Text('Not Available', textStyle);
        this.unavailableText.x = 302 + (1.5 * slotWidth);
        this.unavailableText.y = 8 + slotHeight;
        this.DescriptionContainer.addChild(this.unavailableText);

        this.addChild(this.DescriptionContainer);
    }

    getAvailability() {
        let avail = "";
        for (let i = 0; i < 5; i++) {
            for (let j = 0; j < 48; j++) {
                avail += this.TimeSlots[i][j].Available ? "Y" : "N";
            }
        }

        return avail;
    }

    setAvailability(avail) {
        if (avail.length != 5 * 48)
            throw "Invalid availability string";

        for (let i = 0; i < 5; i++) {
            for (let j = 0; j < 48; j++) {
                if (avail.charAt((i * 48) + j) === "Y" || avail.charAt((i * 48) + j) === "N")
                    this.TimeSlots[i][j].setAvailability(avail.charAt((i * 48) + j) === "Y");
                else {
                    throw "Invalid char in availability string";
                }
            }
        }

        this.resetDirty();
    }

    setDirtyCallback(callback) {
        this.dirtyCallback = callback;
    }

    onDirty() {
        if (!this.dirty) {
            this.dirty = true;
            console.log("Dirty");
            console.log(this.dirtyCallback);
            if (this.dirtyCallback)
                this.dirtyCallback();
        }
    }

    resetDirty() {
        this.dirty = false;
    }
}
