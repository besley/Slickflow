var canvas = $("#canvas")[0];
if (!canvas.getContext) {
    window.G_vmlCanvasManager.initElement(canvas);
}
var ctx = canvas.getContext("2d");
var cellMap = {};
var connectorMap = {};
var connectorNodeMap = {};

function drawWf() {
    var rowHeight = 40, rowpadding = 30, rowWidth = 120, interval = 10, canvasWidth = canvas.width, canvasHeight = canvas.height,
                row = 3,
                col = 3,
                startRow = 1,
                startCol = 1,
                x = (col - 1) * (rowWidth + rowpadding * 2 + interval) + rowpadding,
                y = (row - 1) * (rowHeight + rowpadding * 2 + interval) + rowpadding;
    var xml = null;

    //准备活动集合
    var xmlStr = $("#wfxml").html().replace(/&lt;/g, "<").replace(/&gt;/g, ">");
    xml = $.parseXML(xmlStr);

    var nodeId, type;
    var m = 1, height = rowHeight, width = rowWidth, name = '';
    $.each($(xml).find("[id]"), function (i, node) {
        if (node.tagName == "Activity") {
            row = 1; col = 2;
            nodeId = $(node).attr("id");
            type = $($(node).find("ActivityType")[0]).attr("type");
            var Location = $(node).find("Location")[0];
            if (type == "StartNode") {
            } else {
            }
            row = m;
            x = (col - 1) * (rowWidth + rowpadding * 2 + interval) + rowpadding;
            y = (row - 1) * (rowHeight + rowpadding * 2 + interval) + rowpadding;

            // y = rowHeight + 80 * (m + 1);
            name = $(node).attr("name");
            /*
            width = parseInt($(Location).attr("Width"));
            height = parseInt($(Location).attr("Height"));
            x = parseInt($(Location).attr("Left"));
            y = parseInt($(Location).attr("Top"));
            */
            console.log(name + ":x=" + x + " y=" + y + " width=" + width + " height=" + height);
            cellMap[nodeId] = {
                id: nodeId,
                name: name,
                type: type,
                width: width,
                height: height,
                x: x,
                y: y,
                row: row,
                col: col
            };
            m++;
        } else if (node.tagName == "Transition") {
            if (node.tagName == "Transition") {
                connectorNodeMap[$(node).attr("id")] = node;
            }
        }
    });

    //处理分支下的步骤
    $.each(cellMap, function (id, cell) {
        var j = 1;
        if (cell.type == "GatewayNode") {
            var iCell;
            var i = 1;
            $.each(connectorNodeMap, function (ids, connectorNode) {
                connectorNode = $(connectorNode);
                var fromId = connectorNode.attr("from");
                var toId = connectorNode.attr("to");
                if (id == fromId) {
                    if (i > 1) {
                        row = iCell.row + 1;
                        col = iCell.col + 1;

                        x = (col - 1) * (rowWidth + rowpadding * 2 + interval) + rowpadding;
                        y = (row - 1) * (rowHeight + rowpadding * 2 + interval) + rowpadding;
                        cellMap[toId].row = row;
                        cellMap[toId].col = col;
                        cellMap[toId].x = x;
                        cellMap[toId].y = y;

                    } else {

                    }
                    iCell = cellMap[toId];
                    i++;
                }
            });


            j++;
        }
    });


    $.each(connectorNodeMap, function (id, connectorNode) {
        connectorNode = $(connectorNode);
        var srcId = connectorNode.attr("from");
        var targetId = connectorNode.attr("to");
        var srcNode = cellMap[srcId];
        var targetNode = cellMap[targetId];
        var interval = 20, from = 0, to = 0, routings = [];
        var label = $(connectorNode.find("Description")[0]).text();

        // 手动计算from、to、routings
        // src在target的左侧列
        if (srcNode.col < targetNode.col) {
            from = 5;
            to = 4;

            // 无需绕路而行，routings: (src_x+20, src_y), (src_x+20, target_y)
            var flag = false;
            if (srcNode.col < targetNode.col - 1) {
                // 判断是否穿过了某个cell
                $.each(cellMap, function (id, cell) {
                    if (cell.row == targetNode.row && cell.col < targetNode.col && cell.col > srcNode.col) {
                        flag = true;
                        return;
                    }
                });
            }
            // 没有穿过
            if (!flag) {
                if (srcNode.row != targetNode.row) {
                    routings.push({ x: srcNode.x + srcNode.width + interval, y: srcNode.y + srcNode.height / 2 });
                    routings.push({ x: srcNode.x + srcNode.width + interval, y: targetNode.y + targetNode.height / 2 });
                }
                // 同一行上，直连，没有任何的routing
            } else {
                if (srcNode.row > targetNode.row) {
                    routings.push({ x: srcNode.x + srcNode.width + interval, y: srcNode.y + srcNode.height / 2 });
                    routings.push({ x: srcNode.x + srcNode.width + interval, y: targetNode.y + targetNode.height + interval });
                    routings.push({ x: targetNode.x - interval, y: targetNode.y + targetNode.height + interval });
                    routings.push({ x: targetNode.x - interval, y: targetNode.y + targetNode.height / 2 });
                } else if (srcNode.row == targetNode.row) {
                    from = 7;
                    routings.push({ x: srcNode.x + srcNode.width / 2, y: srcNode.y + srcNode.height + interval });
                    routings.push({ x: targetNode.x - interval, y: targetNode.y + targetNode.height + interval });
                    routings.push({ x: targetNode.x - interval, y: targetNode.y + targetNode.height / 2 });
                } else {
                    from = 7;
                    to = 2;
                    routings.push({ x: srcNode.x + srcNode.width / 2, y: srcNode.y + srcNode.height + interval });
                    routings.push({ x: targetNode.x + targetNode.width / 2, y: srcNode.y + srcNode.height + interval });
                }
            }
        } else if (srcNode.col == targetNode.col) {
            if (srcNode.row < targetNode.row) {
                var flag = false;
                if (srcNode.row < targetNode.row - 1) {
                    // 判断是否穿过了某个cell
                    $.each(cellMap, function (id, cell) {
                        if (cell.col == targetNode.col && cell.row < targetNode.row && cell.row > srcNode.row) {
                            flag = true;
                            return;
                        }
                    });
                }
                // 穿过了
                from = 7;
                to = 2;
                if (flag) {
                    to = 4;
                    routings.push({ x: srcNode.x + srcNode.width / 2, y: srcNode.y + srcNode.height + interval });
                    routings.push({ x: srcNode.x - interval, y: srcNode.y + srcNode.height + interval });
                    routings.push({ x: srcNode.x - interval, y: targetNode.y + targetNode.height / 2 });
                } else {
                    // no routings，直连
                }
            } else if (srcNode.row > targetNode.row) {
                from = 7;
                to = 2;
                routings.push({ x: srcNode.x + srcNode.width / 2, y: srcNode.y + srcNode.height + interval });
                routings.push({ x: srcNode.x - interval, y: srcNode.y + srcNode.height + interval });
                routings.push({ x: srcNode.x - interval, y: targetNode.y - interval });
                routings.push({ x: srcNode.x + srcNode.width / 2, y: targetNode.y - interval });
            }

        } else { // srcNode.col > targetNode.col
            if (srcNode.row >= targetNode.row) {
                from = 5;
                to = 2;
                routings.push({ x: srcNode.x + srcNode.width + interval, y: srcNode.y + srcNode.height / 2 });
                routings.push({ x: srcNode.x + srcNode.width + interval, y: targetNode.y - interval });
                routings.push({ x: targetNode.x + targetNode.width / 2, y: targetNode.y - interval });
            } else {
                var flag = false;
                // 判断是否穿过了某个cell
                $.each(cellMap, function (id, cell) {
                    if (cell.col == srcNode.col && cell.row < targetNode.row && cell.row > srcNode.row) {
                        flag = true;
                        return;
                    }
                });

                if (!flag) {
                    from = 7;
                    to = 2;
                    routings.push({ x: srcNode.x + srcNode.width / 2, y: targetNode.y - interval });
                    routings.push({ x: targetNode.x + targetNode.width / 2, y: targetNode.y - interval });
                } else {
                    from = 5;
                    to = 2;
                    routings.push({ x: srcNode.x + srcNode.width + interval, y: srcNode.y + srcNode.height / 2 });
                    routings.push({ x: srcNode.x + srcNode.width + interval, y: targetNode.y - interval });
                    routings.push({ x: targetNode.x + targetNode.width / 2, y: targetNode.y - interval });
                }
            }
        }

        connectorMap[id] = {
            id: id,
            label: label, //actionNode.attr("name"),
            // color : connectorNode.attr("color"),
            linewidth: parseFloat(connectorNode.attr("linewidth")) || 1,
            labelx: parseFloat(connectorNode.attr("labelx")) || 500,
            labely: parseFloat(connectorNode.attr("labely")) || 500,
            from: from,
            to: to,
            src: cellMap[srcId],
            target: cellMap[targetId],
            routings: routings
        };
    });




    $.each(cellMap, function (id, cell) {
        drawCell(cell);
    });


    $.each(connectorMap, function (id, connector) {
        drawConnector(connector);
    });


}
var borderColor = "Black", stepColor = "#FFFF37", fontSize = 15, fontFamily = "Arial", shadowOffset = 6, shadwColor = "#d0d0d0",
    fontColor = "Black", lineColor = "Black", labelFontSize = 12;
var font = "normal normal bold " + fontSize + "px " + fontFamily, labelFont = "normal normal normal " + labelFontSize + "px " + fontFamily;


/**
		 * 画连接
		 * @param c
		 */
function drawConnector(c) {
    var points = [];
    // 起点
    points.push(getPortPoint(c.src, c.from));


    // 中间点
    $.each(c.routings, function (idx, routing) {
        points.push(routing);
    });
    // 终点
    points.push(getPortPoint(c.target, c.to));

    var last = points.length - 1;
    // 如果两端是cell的中点，就获取与cell相交的点
    if (c.from == 0) {
        points[0] = getCrossPoint(c.src, points[1], points[0]);
    };
    if (c.to == 0) {
        points[last] = getCrossPoint(c.target, points[last - 1], points[last]);
    };

    // 画线
    ctx.strokeStyle = lineColor;
    for (var i = 0; i < last; i++) {
        drawLine(points[i], points[i + 1]);
    }

    // 画文字
    var dx = points[last].x - points[0].x,
        dy = points[last].y - points[0].y,
        fx = c.labelx / 1000 * dx + points[0].x,
        fy = c.labely / 1000 * dy + points[0].y,
        fw = c.label.length * labelFontSize,
        fh = labelFontSize;

    fx -= fw / 2;
    fy += fh / 2;



    //label显示在最长的points上
    var max_index = -1, max_len = 0, tmp_len;
    for (var i = 1; i < points.length; i++) {
        tmp_len = Math.sqrt(Math.pow(Math.abs(points[i].x - points[i - 1].x), 2) + Math.pow(Math.abs(points[i].y - points[i - 1].y), 2));
        if (tmp_len > max_len) {
            max_len = tmp_len;
            max_index = i;
        }
    }
    if (max_index > 0) {
        fx = Math.abs(points[max_index].x - points[max_index - 1].x) / 2 + points[max_index - 1].x;
        fy = Math.abs(points[max_index].y - points[max_index - 1].y) / 2 + points[max_index - 1].y;
    }

    ctx.textAlign = "center";
    ctx.font = labelFont;
    ctx.fillStyle = fontColor;
    ctx.fillText(c.label, fx, fy);

    // 画箭头
    drawArrow(points[last - 1], points[last]);

};



function getPortPoint(cell, no) {
    var x = cell.x,
        y = cell.y;
    switch (no) {
        case 1:
            x = cell.x;
            y = cell.y;
            break;
        case 2:
            x += cell.width / 2;
            break;
        case 3:
            x += cell.width;
            break;
        case 4:
            y += cell.height / 2;
            break;
        case 5:
            x += cell.width;
            y += cell.height / 2;
            break;
        case 6:
            y += cell.height;
            break;
        case 7:
            x += cell.width / 2;
            y += cell.height;
            break;
        case 8:
            x += cell.width;
            y += cell.height;
            break;
        case 0:
            x += cell.width / 2;
            y += cell.height / 2;
            break;
    }
    return { x: x, y: y };
}



/**
		 * 画线
		 * @param p1
		 * @param p2
		 */
function drawLine(p1, p2) {
    ctx.beginPath();
    ctx.moveTo(p1.x, p1.y);
    ctx.lineTo(p2.x, p2.y);
    ctx.stroke();
}


/**
 * 画箭头
 * @param p1
 * @param p2
 */
function drawArrow(p1, p2) {
    var
        awrad = Math.PI / 6, // 箭头角度（30度）
        arrowLen = 10,      // 箭头长度
        ap0 = toRelative(p1, p2), // 旋转源点（line.p1相对于line.p2的坐标）
        ap1 = rotateVec(ap0, awrad, arrowLen), // 第一端点（相对于line.p2的坐标）
        ap2 = rotateVec(ap0, -awrad, arrowLen); // 第二端点（相对于line.p2的坐标）

    ap1 = toAbsolute(ap1, p2);
    ap2 = toAbsolute(ap2, p2);

    drawLine(p2, ap1);
    drawLine(p2, ap2);
}

// 转换成相对坐标
function toRelative(p, p0) {
    return {
        x: p.x - p0.x,
        y: p.y - p0.y
    };
}

// 转换回绝对坐标
function toAbsolute(p, p0) {
    return {
        x: p.x + p0.x,
        y: p.y + p0.y
    };
}


/**
 * 矢量旋转函数
 * @param p 坐标源点
 * @param ang 旋转角
 * @param newLen 新长度
 * @returns {x,y}
 */
function rotateVec(p, ang, newLen) {
    var vx = p.x * Math.cos(ang) - p.y * Math.sin(ang);
    var vy = p.x * Math.sin(ang) + p.y * Math.cos(ang);
    var d = Math.sqrt(vx * vx + vy * vy);
    if (Math.abs(d) > 0) {
        vx = vx / d * newLen;
        vy = vy / d * newLen;
    }
    return {
        x: vx,
        y: vy
    };
}

/**
 * 画步骤
 * @param cell
 */
function drawCell(cell) {
    ctx.strokeStyle = borderColor;
    ctx.fillStyle = stepColor;

    drawRect(cell, cell.width, cell.height);
    //var fx = cell.x + (cell.width - cell.name.length * fontSize) / 2;
    var fy = cell.y + (cell.height - fontSize) / 2 + fontSize;
    var fx = cell.x + cell.width / 2;
    console.log("fx=" + fx + " cell.x" + cell.x + " cell.width=" + cell.width);
    ctx.textAlign = "center";
    ctx.font = font;
    ctx.fillStyle = fontColor;
    ctx.fillText(cell.name, fx, fy);

}



/**
 * 画圆角方块
 * @param p 坐标点
 * @param w 宽度
 * @param h 高度
 * @param fill 是否填充
 * @param stroke 是否画线
 * @param drawShadow 是否画阴影
 */
function drawRect(p, w, h, fill, stroke, drawShadow) {
    fill = typeof (fill) == "undefined" ? true : fill;
    stroke = typeof (stroke) == "undefined" ? true : stroke;
    drawShadow = typeof (drawShadow) == "undefined" ? true : drawShadow;
    /*
    if (drawShadow) {
        var offset = shadowOffset;
        var oldStyle = ctx.fillStyle;
        ctx.fillStyle = shadwColor;
        drawRect({ x: p.x + offset, y: p.y + offset }, w, h, true, false, false);
        ctx.fillStyle = oldStyle;
    }*/
    var x = p.x,
        y = p.y,
        r = 5; // 圆角半径
    if (w < 2 * r) {
        r = w / 2;
    }
    if (h < 2 * r) {
        r = h / 2;
    }
    ctx.beginPath();
    ctx.moveTo(x + r, y);
    ctx.lineTo(x + w - r, y);
    ctx.quadraticCurveTo(x + w, y, x + w, y + r);
    ctx.lineTo(x + w, y + h - r);
    ctx.quadraticCurveTo(x + w, y + h, x + w - r, y + h);
    ctx.lineTo(x + r, y + h);
    ctx.quadraticCurveTo(x, y + h, x, y + h - r);
    ctx.lineTo(x, y + r);
    ctx.quadraticCurveTo(x, y, x + r, y);
    ctx.closePath();

    if (stroke) {
        ctx.stroke();
    }
    if (fill) {
        ctx.fill();
    }


};














