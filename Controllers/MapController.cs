using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using RoadAppWEB.Data;
using RoadAppWEB.Models;
using RoadAppWEB.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper.Configuration;
using System.IO;
using OfficeOpenXml.Table;
using System.Drawing;

namespace RoadAppWEB.Controllers
{
    public class MapController : Controller
    {
        private readonly RoadAppWEBContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MapController(RoadAppWEBContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // 
        // GET: /Map/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BaiduMap(string site, FacilityRoadNode viewModel, string selectedFacility, string selectedRoad, string selectedType)
        {
            var nodeIdToSelect = "JuRuibin00";
            var nodes = from n in _context.node
                        where n.id == nodeIdToSelect
                        select n;

            if (!string.IsNullOrWhiteSpace(selectedFacility))
            {
                if (selectedFacility == "全部")
                {
                    // 查询所有节点
                    nodes = from n in _context.node
                            select n;
                }
                else
                {
                    // 查询所有符合条件的道路
                    var roadList = (from r in _context.road
                                    where r.facility_id == selectedFacility
                                    select r.id).ToList();
                    if (!string.IsNullOrWhiteSpace(selectedType))
                    {
                        roadList = null;
                        // 查询所有符合条件的道路
                        roadList = (from r in _context.road
                                        where r.facility_id == selectedFacility && r.type == selectedType
                                        select r.id).ToList();
                    }
                    // 查询所有节点，其中 road_id 在 roadList 中或等于 selectedFacility
                    nodes = from n in _context.node
                            where roadList.Contains(n.road_id)
                            select n;
                }
            }
            if (!string.IsNullOrWhiteSpace(selectedRoad))
            {
                // 查询所有符合条件的道路
                var roadSelected = (from r in _context.road
                                   where r.id == selectedRoad
                                   select r.id).ToList();

                // 查询所有节点，其中 road_id 在 roadSelected 中或等于 selectedFacility
                nodes = from n in _context.node
                        where roadSelected.Contains(n.road_id)
                        select n;
            }
            var roads = from r in _context.road
                        select r;
            var facilities = from f in _context.facility
                             select f;

            if (site == null)
            {
                ViewData["site"] = "上海市";
            }
            else
            {
                ViewData["site"] = site;
            }
            ViewData["SelectedNavItem"] = "BaiduMap";
            ViewData["facility"] = selectedFacility;
            ViewData["roads"] = selectedRoad;
            ViewData["facility_list"] = facilities;
            ViewData["selectedType"] = selectedType;
            // 将节点和 Facility 数据存储在 viewModel 中
            viewModel.Nodes = await nodes.ToListAsync();
            viewModel.Facilities = await facilities.ToListAsync();
            viewModel.Roads = await roads.ToListAsync();

            return View(viewModel);
        }


        public async Task<IActionResult> InfraData()
        {
            ViewData["SelectedNavItem"] = "InfraData";
            return View();
        }
        public async Task<IActionResult> SystemSettings()
        {
            ViewData["SelectedNavItem"] = "SystemSettings";
            return View();
        }
        [HttpGet]
        public IActionResult GetSelectedNode(string id)
        {
            // 假设你使用 Entity Framework Core 来访问数据库
            // 这里需要根据你的数据访问逻辑来查询选定节点的数据
            // 解码传递的 id 参数
            var decodedId = WebUtility.UrlDecode(id);
            var selectedNodeData = _context.node.FirstOrDefault(node => node.id == decodedId);

            // 检查是否找到了选定节点的数据
            if (selectedNodeData != null)
            {
                // 将选定节点的数据传递给编辑页面的视图
                return Json(selectedNodeData);
            }
            else
            {
                // 如果未找到选定节点的数据，可以返回适当的错误信息或状态码
                return NotFound("未找到选定的节点数据");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,hubnode,fathernode,childnode,mainline_id,ramp_id,level,longitude,latitude")] node node)
        {
            if (ModelState.IsValid)
            {
                _context.Add(node);

                //更新相关节点的信息
                if (!string.IsNullOrEmpty(node.fathernode))
                {
                    // 查找以fathernode为id的节点
                    var parentNode = await _context.node.FindAsync(node.fathernode);
                    if (parentNode != null)
                    {
                        // 更新父节点的childnode信息
                        if (string.IsNullOrEmpty(parentNode.childnode))
                        {
                            parentNode.childnode = node.id;
                        }
                        else
                        {
                            parentNode.childnode += ";" + node.id;
                        }

                        // 保存父节点的更改
                        _context.Update(parentNode);
                    }
                }

                if (!string.IsNullOrEmpty(node.childnode))
                {
                    // 查找以childnode为id的节点
                    var childNode = await _context.node.FindAsync(node.childnode);
                    if (childNode != null)
                    {
                        // 更新父节点的childnode信息
                        if (string.IsNullOrEmpty(childNode.fathernode))
                        {
                            childNode.fathernode = node.id;
                        }
                        else
                        {
                            childNode.fathernode += ";" + node.id;
                        }

                        // 保存父节点的更改
                        _context.Update(childNode);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BaiduMap));
            }
            return View(node);
        }
        public async Task<IActionResult> NodeEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the node data by ID from your database or data source
            var node = await _context.node.FindAsync(id);

            if (node == null)
            {
                return NotFound();
            }
            // Pass the single node as the model to the view
            return RedirectToAction(nameof(BaiduMap));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NodeEdit(string id, [Bind("id,hubnode,fathernode,childnode,road_id,level,longitude,latitude")] node node)
        {
            if (id != node.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentNode = await _context.node.FindAsync(id);
                    if (currentNode.fathernode != node.fathernode)
                    {

                    }
                    if (currentNode.childnode != node.fathernode)
                    {

                    }
                    _context.Update(node);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!nodeExists(node.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BaiduMap));
            }
            return RedirectToAction(nameof(BaiduMap));
        }

        private bool nodeExists(string id)
        {
            return (_context.node?.Any(e => e.id == id)).GetValueOrDefault();
        }

        // POST: Map/Delete/5
        [HttpPost, ActionName("NodeDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NodeDeleteConfirmed(string id)
        {
            if (_context.node == null)
            {
                return Problem("Entity set 'RoadAppWEBContext.node' is null.");
            }

            var node = await _context.node.FindAsync(id);
            if (node != null)
            {
                // 获取父节点和子节点的ID列表
                var parentNodeIds = !string.IsNullOrEmpty(node.fathernode) ? node.fathernode.Split(';') : new string[0];
                var childNodeIds = !string.IsNullOrEmpty(node.childnode) ? node.childnode.Split(';') : new string[0];

                // 更新父节点的 childnode 信息
                foreach (var parentId in parentNodeIds)
                {
                    var parentNode = await _context.node.FindAsync(parentId);
                    if (parentNode != null)
                    {
                        // 从父节点的 childnode 中移除被删除节点的ID
                        parentNode.childnode = string.Join(";", parentNode.childnode.Split(';').Where(id => id != node.id));
                        _context.Update(parentNode);
                    }
                }

                // 更新子节点的 fathernode 信息
                foreach (var childId in childNodeIds)
                {
                    var childNode = await _context.node.FindAsync(childId);
                    if (childNode != null)
                    {
                        // 从子节点的 fathernode 中移除被删除节点的ID
                        childNode.fathernode = string.Join(";", childNode.fathernode.Split(';').Where(id => id != node.id));
                        _context.Update(childNode);
                    }
                }

                _context.node.Remove(node);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(BaiduMap));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoadSegments([Bind("id,hubnode,fathernode,childnode,mainline_id,ramp_id,level,longitude,latitude")] node node)
        {
            return RedirectToAction(nameof(BaiduMap));
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0)
            {
                return BadRequest("未选择文件或文件为空。");
            }

            // 在这里，您可以使用库（如EPPlus）来读取XLSX文件的内容
            // 这里只是一个示例，您需要根据实际需求进行更详细的处理

            using (var stream = file.OpenReadStream())
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets["桩号数据"];

                if (worksheet != null)
                {
                    // 读取XLSX的每一列并进行处理示例
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    List<facility> facilities = new List<facility>();
                    List<road> roads = new List<road>();
                    List<node> nodes = new List<node>();
                    List<List<node>> rampNodeList = new List<List<node>>();
                    List<string> rampName = new List<string>();

                    int nodeCounter = 0; // To track the node order within the road

                    for (int row = 2; row <= rowCount; row++)
                    {
                        // facility
                        var facility_id = worksheet.Cells[row, 2].Value.ToString();
                        var facility_type = "高架";
                        var facility_trunkroad = worksheet.Cells[row, 3].Value.ToString();

                        // road
                        var newroad_id = worksheet.Cells[row, 6].Value?.ToString();
                        if (newroad_id == null)
                        {
                            newroad_id = worksheet.Cells[row, 3].Value?.ToString()+"("+ worksheet.Cells[row, 5].Value?.ToString() + ")";
                        }
                        var newroad_facility_id = worksheet.Cells[row, 2].Value?.ToString();
                        var newroad_type = worksheet.Cells[row, 4].Value?.ToString();
                        var newroad_direction = worksheet.Cells[row, 5].Value?.ToString();

                        // node
                        var id = worksheet.Cells[row, 1].Value.ToString();
                        var road_id = worksheet.Cells[row, 6].Value?.ToString();
                        if (road_id == null)
                        {
                            road_id = worksheet.Cells[row, 3].Value?.ToString() + "(" + worksheet.Cells[row, 5].Value?.ToString() + ")";
                        }
                        var level = 1; // Assuming level is always 1
                        var longitude = Convert.ToDouble(worksheet.Cells[row, 8].Value);
                        var latitude = Convert.ToDouble(worksheet.Cells[row, 9].Value);

                        // Calculate hubnode, fathernode, and childnode based on row number
                        var hubnode = "";
                        var fathernode = "";
                        var childnode = "";
                        var span = Convert.ToDouble(worksheet.Cells[row, 10].Value);
                        // deal with fathernode and childnode
                        var index = Convert.ToInt64(worksheet.Cells[row, 7].Value);
                        var lastIndex = Convert.ToInt64(0);
                        var nextIndex = Convert.ToInt64(0);
                        if (row > 2)
                        {
                            lastIndex = Convert.ToInt64(worksheet.Cells[row - 1, 7].Value);
                        }
                        if (row < rowCount)
                        {
                            nextIndex = Convert.ToInt64(worksheet.Cells[row + 1, 7].Value);
                        }
                        if (row > 2)
                        {
                            // facility and road
                            var newFacility = new facility
                            {
                                id = facility_id,
                                type = facility_type,
                                trunknetwork = facility_trunkroad,
                                date = null,
                                lanes = 1,
                                length = 0,
                                maintenanceunit = null,
                                operatingunit = null,
                                square = null,
                            };
                            var newRoad = new road
                            {
                                id = newroad_id,
                                avg_length = 0,
                                code = null,
                                direction = newroad_direction,
                                end_linknode = null,
                                end_node = null,
                                facility_id = newroad_facility_id,
                                start_linknode = null,
                                start_node = worksheet.Cells[row, 1].Value.ToString(),
                                type = newroad_type,
                            };
                            // 查找新节点的 newFacility.id 是否已存在于 facilities
                            bool existsInFacilities = facilities.Any(f => f.id == newFacility.id);

                            if (!existsInFacilities)
                            {
                                facilities.Add(newFacility);
                            }
                            // 查找新节点的 newRoad.id 是否已存在于 roads
                            bool existsInRoads = roads.Any(r => r.id == newRoad.id);

                            if (!existsInRoads)
                            {
                                roads.Add(newRoad);
                            }

                            // node
                            // new road
                            if (lastIndex > index)
                            {
                                childnode = worksheet.Cells[row + 1, 1].Value.ToString();
                                nodeCounter++;
                                
                            }
                            else
                            {
                                if (index > nextIndex)
                                {
                                    fathernode = worksheet.Cells[row - 1, 1].Value.ToString();
                                    nodeCounter++;
                                    if (worksheet.Cells[row, 6].Value != null)
                                    {
                                        // 查找新节点的 newRoad.id 是否已存在于 roads
                                        bool flagTemp = roads.Any(r => r.id == worksheet.Cells[row, 6].Value.ToString());

                                        if (flagTemp)
                                        {
                                            roads.FirstOrDefault(r => r.id == worksheet.Cells[row, 6].Value.ToString()).end_node = worksheet.Cells[row, 1].Value.ToString();
                                        }
                                    }
                                    else
                                    {
                                        // 查找新节点的 newRoad.id 是否已存在于 roads
                                        bool flagTemp = roads.Any(r => r.id == worksheet.Cells[row, 3].Value?.ToString() + "(" + worksheet.Cells[row, 5].Value?.ToString() + ")");

                                        if (flagTemp)
                                        {
                                            roads.FirstOrDefault(r => r.id == worksheet.Cells[row, 3].Value?.ToString() + "(" + worksheet.Cells[row, 5].Value?.ToString() + ")").end_node = worksheet.Cells[row, 1].Value.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    fathernode = worksheet.Cells[row - 1, 1].Value.ToString();
                                    childnode = worksheet.Cells[row + 1, 1].Value.ToString();
                                    nodeCounter++;
                                }
                            }


                            var newNode = new node
                            {
                                id = id,
                                hubnode = hubnode,
                                fathernode = fathernode,
                                childnode = childnode,
                                road_id = road_id,
                                level = level,
                                longitude = longitude,
                                latitude = latitude,
                                span = span,
                            };

                            // 查找新节点的 newRoad.id 是否已存在于 roads
                            bool existsInNodes = nodes.Any(r => r.id == newNode.id);

                            if (!existsInNodes)
                            {
                                nodes.Add(newNode);
                            }
                        }
                        else
                        {
                            childnode = worksheet.Cells[row + 1, 1].Value.ToString();
                            nodes.Add(new node
                            {
                                id = id,
                                hubnode = hubnode,
                                fathernode = fathernode,
                                childnode = childnode,
                                road_id = road_id,
                                level = level,
                                longitude = longitude,
                                latitude = latitude,
                                span = span
                            });
                            facilities.Add(new facility
                            {
                                id = facility_id,
                                type = facility_type,
                                trunknetwork = facility_trunkroad,
                                date = null,
                                lanes = 1,
                                length = 0,
                                maintenanceunit = null,
                                operatingunit = null,
                                square = null,
                            });
                            roads.Add(new road
                            {
                                id = newroad_id,
                                avg_length = 0,
                                code = null,
                                direction = newroad_direction,
                                end_linknode = null,
                                end_node = null,
                                facility_id = newroad_facility_id,
                                start_linknode = null,
                                start_node = worksheet.Cells[row, 1].Value.ToString(),
                                type = newroad_type,
                            });
                            nodeCounter++;
                        }
                        #region old code
                        //    if (row > 2)
                        //    {
                        //        // facility and road
                        //        var newFacility = new facility
                        //        {
                        //            id = facility_id,
                        //            type = facility_type,
                        //            trunknetwork = facility_trunkroad,
                        //            date = null,
                        //            lanes = 1,
                        //            length = 0,
                        //            maintenanceunit = null,
                        //            operatingunit = null,
                        //            square = null,
                        //        };
                        //        var newRoad = new road
                        //        {
                        //            id = newroad_id,
                        //            avg_length = 0,
                        //            code = null,
                        //            direction = newroad_direction,
                        //            end_linknode = null,
                        //            end_node = null,
                        //            facility_id = newroad_facility_id,
                        //            start_linknode = null,
                        //            start_node = null,
                        //            type = newroad_type,
                        //        };
                        //        // 查找新节点的 newFacility.id 是否已存在于 facilities
                        //        bool existsInFacilities = facilities.Any(f => f.id == newFacility.id);

                        //        if (!existsInFacilities)
                        //        {
                        //            facilities.Add(newFacility);
                        //        }
                        //        // 查找新节点的 newRoad.id 是否已存在于 roads
                        //        bool existsInRoads = roads.Any(r => r.id == newRoad.id);

                        //        if (!existsInRoads)
                        //        {
                        //            roads.Add(newRoad);
                        //        }

                        //        // node
                        //        var indexLast = Convert.ToInt64(worksheet.Cells[row - 1, 7].Value);
                        //        var indexNext = Convert.ToInt64(worksheet.Cells[row + 1, 7].Value);

                        //        if (currentRoadTypeDirection == "")
                        //        {
                        //            currentRoadTypeDirection = worksheet.Cells[row, 3].Value?.ToString() + worksheet.Cells[row, 4].Value?.ToString() + worksheet.Cells[row, 5].Value?.ToString();
                        //        }
                        //        else if (currentRoadTypeDirection != worksheet.Cells[row, 3].Value?.ToString() + worksheet.Cells[row, 4].Value?.ToString() + worksheet.Cells[row, 5].Value?.ToString())
                        //        {
                        //            foreach (List<node> list in rampNodeList)
                        //            {
                        //                nodes.AddRange(list);
                        //                nodeCounter = nodes.Count;
                        //            }

                        //            nodeIdx0 = 0;
                        //            nodeIdx1 = 0;
                        //            rampNodeList.Clear();
                        //            rampName.Clear();
                        //            currentRoadTypeDirection = worksheet.Cells[row, 3].Value?.ToString() + worksheet.Cells[row, 4].Value?.ToString() + worksheet.Cells[row, 5].Value?.ToString();
                        //        }

                        //        var newNode = new node
                        //        {
                        //            id = id,
                        //            hubnode = hubnode,
                        //            fathernode = fathernode,
                        //            childnode = childnode,
                        //            road_id = road_id,
                        //            level = level,
                        //            longitude = longitude,
                        //            latitude = latitude
                        //        };

                        //        // 查找新节点的 road_id 是否已存在于 rampName
                        //        var existingIndex = rampName.IndexOf(newNode.road_id);

                        //        if (existingIndex >= 0)
                        //        {
                        //            // Check if the node already exists in the list
                        //            // 如果不存在
                        //            if (!rampNodeList[existingIndex].Any(n => n.id == newNode.id))
                        //            {
                        //                newNode.fathernode = rampNodeList[existingIndex][rampNodeList[existingIndex].Count - 1].id;
                        //                rampNodeList[existingIndex][rampNodeList[existingIndex].Count - 1].childnode = newNode.id;
                        //                // 如果已经存在，将 newNode 添加到相应索引的 rampNodeList
                        //                rampNodeList[existingIndex].Add(newNode);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            // 如果不存在，添加到 rampName，并创建新的列表
                        //            rampName.Add(newNode.road_id);
                        //            List<node> newList = new List<node>{
                        //                        newNode
                        //                    };
                        //            rampNodeList.Add(newList);
                        //        }
                        //    //}
                        //    //else
                        //    //{
                        //    //    childnode = worksheet.Cells[row + 1, 1].Value.ToString();
                        //    //    nodes.Add(new node
                        //    //    {
                        //    //        id = id,
                        //    //        hubnode = hubnode,
                        //    //        fathernode = fathernode,
                        //    //        childnode = childnode,
                        //    //        road_id = road_id,
                        //    //        level = level,
                        //    //        longitude = longitude,
                        //    //        latitude = latitude
                        //    //    });
                        //    //    facilities.Add(new facility
                        //    //    {
                        //    //        id = facility_id,
                        //    //        type = facility_type,
                        //    //        trunknetwork = facility_trunkroad,
                        //    //        date = null,
                        //    //        lanes = 1,
                        //    //        length = 0,
                        //    //        maintenanceunit = null,
                        //    //        operatingunit = null,
                        //    //        square = null,
                        //    //    });
                        //    //    roads.Add(new road
                        //    //    {
                        //    //        id = newroad_id,
                        //    //        avg_length = 0,
                        //    //        code = null,
                        //    //        direction = newroad_direction,
                        //    //        end_linknode = null,
                        //    //        end_node = null,
                        //    //        facility_id = newroad_facility_id,
                        //    //        start_linknode = null,
                        //    //        start_node = null,
                        //    //        type = newroad_type,
                        //    //    });
                        //    //    nodeCounter++;
                        //    //}
                        //}
                        #endregion
                    }
                    await createFacilityFromList(facilities);
                    await createRoadFromList(roads);
                    await createNodeFromList(nodes);
                }

                var worksheet2 = package.Workbook.Worksheets["路段关联数据"];

                if (worksheet2 != null)
                {
                    // 创建用于存储数据的列表
                    var fatherNodeList = new List<node>();
                    var childNodeList = new List<node>();

                    // 获取工作表的总行数
                    var rowCount = worksheet2.Dimension.Rows;

                    // 从第二行开始遍历工作表，因为第一行通常是标题
                    for (int row = 2; row <= rowCount; row++)
                    {
                        // 读取第三列和第五列的数据，并存储到相应的列表中
                        var fatherNodeID = worksheet2.Cells[row, 3].Text;
                        var childNodeID = worksheet2.Cells[row, 5].Text;

                        var fatherNode = await _context.node.FindAsync(fatherNodeID);
                        var childNode = await _context.node.FindAsync(childNodeID);

                        if (fatherNode != null && childNode != null)
                        {
                            fatherNodeList.Add(fatherNode);
                            childNodeList.Add(childNode);
                        }
                    }
                    // 在这里使用 fatherNodeList
                    await linkNodesFromList(fatherNodeList, childNodeList);
                }
            }

            if (true)
            {
                // HubNode
                List<HubNodeRes> hubNodeReses = new List<HubNodeRes>();
                var roads = from r in _context.road
                            select r;
                var index = 1;
                foreach (road roadTemp in roads)
                {
                    // 主路段
                    var id = index++;
                    var starthub_id = roadTemp.start_node;
                    var endhub_id = roadTemp.end_node;
                    double span = 0;
                    var nodesFromRoad = from n in _context.node
                                        where n.road_id == roadTemp.id
                                        select n;
                    foreach (node n in nodesFromRoad)
                    {
                        // 如果 n.span 不为 null，则使用其值，否则使用 0.0
                        span += (n.span != null) ? n.span.Value : 0.0;
                    }
                    var velocity = 30;
                    var direction = roadTemp.type;
                    var type = roadTemp.type;

                    HubNodeRes newHubNodeRes = new HubNodeRes
                    {
                        id = id,
                        starthub_id = starthub_id,
                        endhub_id = endhub_id,
                        span = span,
                        direction = direction,
                        velocity = velocity,
                        type = type,
                    };
                    hubNodeReses.Add(newHubNodeRes);

                    // 关联路段
                    if (roadTemp.type == "主线")
                    {
                        id = index++;
                        starthub_id = roadTemp.end_node;
                        endhub_id = roadTemp.end_linknode;
                        span = 0;
                        velocity = 30;
                        direction = roadTemp.type;
                        type = roadTemp.type + "-" + roadTemp.type;

                        newHubNodeRes = new HubNodeRes
                        {
                            id = id,
                            starthub_id = starthub_id,
                            endhub_id = endhub_id,
                            span = span,
                            direction = direction,
                            velocity = velocity,
                            type = type,
                        };
                        hubNodeReses.Add(newHubNodeRes);
                    }
                    else if (roadTemp.type == "匝道")
                    {
                        id = index++;
                        if (roadTemp.end_linknode != null && roadTemp.end_linknode != "")
                        {
                            starthub_id = roadTemp.end_node;
                            endhub_id = roadTemp.end_linknode;
                            direction = roadTemp.type;
                            type = roadTemp.type + "-" + "主线";
                        }
                        else
                        {
                            starthub_id = roadTemp.start_linknode;
                            endhub_id = roadTemp.start_node;
                            type = "主线" + "-" + roadTemp.type;
                            direction = roadTemp.type;
                        }
                        span = 0;
                        velocity = 30;

                        newHubNodeRes = new HubNodeRes
                        {
                            id = id,
                            starthub_id = starthub_id,
                            endhub_id = endhub_id,
                            span = span,
                            direction = direction,
                            velocity = velocity,
                            type = type,
                        };
                        hubNodeReses.Add(newHubNodeRes);
                    }
                    else if (roadTemp.type == "立交")
                    {
                        if (roadTemp.end_linknode != null && roadTemp.end_linknode != "")
                        {
                            if (roadTemp.end_linknode.Contains(';'))
                            {
                                foreach (string temp_end_linknode in roadTemp.end_linknode.Split(';'))
                                {
                                    id = index++;
                                    starthub_id = roadTemp.end_node;
                                    endhub_id = temp_end_linknode;

                                    var linknode = _context.node.FirstOrDefault(node => node.id == endhub_id);
                                    var linknode_type = _context.road.FirstOrDefault(road => road.id == linknode.road_id);
                                    direction = roadTemp.type;
                                    type = roadTemp.type + "-" + linknode_type.type;

                                    span = 0;
                                    velocity = 30;

                                    newHubNodeRes = new HubNodeRes
                                    {
                                        id = id,
                                        starthub_id = starthub_id,
                                        endhub_id = endhub_id,
                                        span = span,
                                        direction = direction,
                                        velocity = velocity,
                                        type = type,
                                    };
                                    hubNodeReses.Add(newHubNodeRes);
                                }
                            }
                            else
                            {
                                id = index++;
                                starthub_id = roadTemp.end_node;
                                endhub_id = roadTemp.end_linknode;

                                var linknode = _context.node.FirstOrDefault(node => node.id == endhub_id);
                                var linknode_type = _context.road.FirstOrDefault(road => road.id == linknode.road_id);
                                type = roadTemp.type + "-" + linknode_type.type;
                                direction = roadTemp.type;

                                span = 0;
                                velocity = 30;

                                newHubNodeRes = new HubNodeRes
                                {
                                    id = id,
                                    starthub_id = starthub_id,
                                    endhub_id = endhub_id,
                                    span = span,
                                    direction = direction,
                                    velocity = velocity,
                                    type = type,
                                };
                                hubNodeReses.Add(newHubNodeRes);
                            }
                        }
                        if (roadTemp.start_linknode != null && roadTemp.start_linknode != "")
                        {
                            if (roadTemp.start_linknode.Contains(';'))
                            {
                                foreach (string temp_start_linknode in roadTemp.start_linknode.Split(';'))
                                {
                                    id = index++;
                                    starthub_id = temp_start_linknode;
                                    endhub_id = roadTemp.end_node;

                                    var linknode = _context.node.FirstOrDefault(node => node.id == starthub_id);
                                    var linknode_type = _context.road.FirstOrDefault(road => road.id == linknode.road_id);
                                    type = linknode_type.type + "-" + roadTemp.type;
                                    direction = roadTemp.direction;

                                    span = 0;
                                    velocity = 30;

                                    newHubNodeRes = new HubNodeRes
                                    {
                                        id = id,
                                        starthub_id = starthub_id,
                                        endhub_id = endhub_id,
                                        span = span,
                                        direction = direction,
                                        velocity = velocity,
                                        type = type,
                                    };
                                    hubNodeReses.Add(newHubNodeRes);
                                }
                            }
                            else
                            {
                                id = index++;
                                starthub_id = roadTemp.start_linknode;
                                endhub_id = roadTemp.end_node;

                                var linknode = _context.node.FirstOrDefault(node => node.id == starthub_id);
                                var linknode_type = _context.road.FirstOrDefault(road => road.id == linknode.road_id);
                                type = linknode_type.type + "-" + roadTemp.type;
                                direction = roadTemp.direction;

                                span = 0;
                                velocity = 30;

                                newHubNodeRes = new HubNodeRes
                                {
                                    id = id,
                                    starthub_id = starthub_id,
                                    endhub_id = endhub_id,
                                    span = span,
                                    direction = direction,
                                    velocity = velocity,
                                    type = type,
                                };
                                hubNodeReses.Add(newHubNodeRes);
                            }
                        }

                    }


                }

                // AllNode
                List<AllNodesRes> allNodesRes = new List<AllNodesRes>();
                index = 1;
                foreach (road roadTemp in roads)
                {
                    // 主路段
                    double span2hub = 0;
                    double span = 0;
                    var nodesFromRoad = from n in _context.node
                                        where n.road_id == roadTemp.id
                                        select n;
                    foreach (node n in nodesFromRoad)
                    {
                        var id = index++;
                        span = (n.span != null) ? n.span.Value : 0.0;
                        // 如果 n.span 不为 null，则使用其值，否则使用 0.0
                        span2hub += span;
                        var node_id = n.id;
                        var hub_id = roadTemp.end_node;
                        var type = roadTemp.type;

                        var direction = roadTemp.direction;

                        AllNodesRes allNodesRes1 = new AllNodesRes
                        {
                            id = id,
                            node_id = node_id,
                            hub_id = hub_id,
                            span = span,
                            span2hub = span2hub,
                            direction = direction,
                            type = type,
                        };
                        allNodesRes.Add(allNodesRes1);
                    }

                    // 关联路段
                    if (roadTemp.type == "主线")
                    {
                        var id = index++;
                        var node_id = roadTemp.end_node;
                        var hub_id = roadTemp.end_linknode;
                        span = 0;
                        var type = roadTemp.type + "-" + roadTemp.type;
                        var direction = roadTemp.direction;

                        AllNodesRes allNodesRes1 = new AllNodesRes
                        {
                            id = id,
                            node_id = node_id,
                            hub_id = hub_id,
                            span = span,
                            span2hub = span2hub,
                            direction = direction,
                            type = type,
                        };
                        allNodesRes.Add(allNodesRes1);
                    }
                    else if (roadTemp.type == "匝道")
                    {
                        var id = index++;
                        var direction = roadTemp.direction;
                        var node_id = "";
                        var hub_id = "";
                        var type = "";

                        if (roadTemp.end_linknode != null && roadTemp.end_linknode != "")
                        {
                            node_id = roadTemp.end_node;
                            hub_id = roadTemp.end_linknode;
                            type = roadTemp.type + "-" + "主线";
                        }
                        else
                        {
                            node_id = roadTemp.start_linknode;
                            hub_id = roadTemp.start_node;
                            type = "主线" + "-" + roadTemp.type;
                        }
                        span = 0;

                        AllNodesRes allNodesRes1 = new AllNodesRes
                        {
                            id = id,
                            node_id = node_id,
                            hub_id = hub_id,
                            span = span,
                            span2hub = span2hub,
                            direction = direction,
                            type = type,
                        };
                        allNodesRes.Add(allNodesRes1);
                    }
                    else if (roadTemp.type == "立交")
                    {
                        if (roadTemp.end_linknode != null && roadTemp.end_linknode != "")
                        {
                            if (roadTemp.end_linknode.Contains(';'))
                            {
                                foreach (string temp_end_linknode in roadTemp.end_linknode.Split(';'))
                                {
                                    var id = index++;
                                    var node_id = roadTemp.end_node;
                                    var hub_id = temp_end_linknode;
                                    var direction = roadTemp.direction;

                                    var linknode = _context.node.FirstOrDefault(node => node.id == hub_id);
                                    var linknode_type = _context.road.FirstOrDefault(road => road.id == linknode.road_id);
                                    var type = roadTemp.type + "-" + linknode_type.type;

                                    span = 0;

                                    AllNodesRes allNodesRes1 = new AllNodesRes
                                    {
                                        id = id,
                                        node_id = node_id,
                                        hub_id = hub_id,
                                        span = span,
                                        span2hub = span2hub,
                                        direction = direction,
                                        type = type,
                                    };
                                    allNodesRes.Add(allNodesRes1);
                                }
                            }
                            else
                            {
                                var id = index++;
                                var node_id = roadTemp.end_node;
                                var hub_id = roadTemp.end_linknode;
                                var direction = roadTemp.direction;

                                var linknode = _context.node.FirstOrDefault(node => node.id == hub_id);
                                var linknode_type = _context.road.FirstOrDefault(road => road.id == linknode.road_id);
                                var type = roadTemp.type + "-" + linknode_type.type;

                                span = 0;

                                AllNodesRes allNodesRes1 = new AllNodesRes
                                {
                                    id = id,
                                    node_id = node_id,
                                    hub_id = hub_id,
                                    span = span,
                                    span2hub = span2hub,
                                    direction = direction,
                                    type = type,
                                };
                                allNodesRes.Add(allNodesRes1);
                            }
                        }
                        if (roadTemp.start_linknode != null && roadTemp.start_linknode != "")
                        {
                            if (roadTemp.start_linknode.Contains(';'))
                            {
                                foreach (string temp_start_linknode in roadTemp.start_linknode.Split(';'))
                                {
                                    var id = index++;
                                    var node_id = temp_start_linknode;
                                    var hub_id = roadTemp.end_node;
                                    var direction = roadTemp.direction;

                                    var linknode = _context.node.FirstOrDefault(node => node.id == node_id);
                                    var linknode_type = _context.road.FirstOrDefault(road => road.id == linknode.road_id);
                                    var type = linknode_type.type + "-" + roadTemp.type;

                                    span = 0;

                                    AllNodesRes allNodesRes1 = new AllNodesRes
                                    {
                                        id = id,
                                        node_id = node_id,
                                        hub_id = hub_id,
                                        span = span,
                                        span2hub = span2hub,
                                        direction = direction,
                                        type = type,
                                    };
                                    allNodesRes.Add(allNodesRes1);
                                }
                            }
                            else
                            {
                                var id = index++;
                                var node_id = roadTemp.start_linknode;
                                var hub_id = roadTemp.end_node;
                                var direction = roadTemp.direction;

                                var linknode = _context.node.FirstOrDefault(node => node.id == node_id);
                                var linknode_type = _context.road.FirstOrDefault(road => road.id == linknode.road_id);
                                var type = linknode_type.type + "-" + roadTemp.type;

                                span = 0;

                                AllNodesRes allNodesRes1 = new AllNodesRes
                                {
                                    id = id,
                                    node_id = node_id,
                                    hub_id = hub_id,
                                    span = span,
                                    span2hub = span2hub,
                                    direction = direction,
                                    type = type,
                                };
                                allNodesRes.Add(allNodesRes1);
                            }
                        }

                    }
                }
                await createHubNodeResFromList(hubNodeReses);
                await createAllNodeResFromList(allNodesRes);
            }
            
            // 可以根据需要返回适当的视图或重定向
            return RedirectToAction(nameof(BaiduMap));
        }

        public async Task<IActionResult> createNodeFromList(List<node> nodeList)
        {
            if (nodeList != null && nodeList.Count > 0)
            {
                foreach (var node in nodeList)
                {
                    var existingNode = await _context.node.FindAsync(node.id);

                    if (existingNode == null)
                    {
                        // 如果节点不存在，添加它
                        _context.Add(node);
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(BaiduMap));
        }
        public async Task createRoadFromList(List<road> roadList)
        {
            if (roadList != null && roadList.Count > 0)
            {
                foreach (var road in roadList)
                {
                    var existingRode = await _context.road.FindAsync(road.id);

                    if (existingRode == null)
                    {
                        // 如果节点不存在，添加它
                        _context.Add(road);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task createFacilityFromList(List<facility> facilityList)
        {
            if (facilityList != null && facilityList.Count > 0)
            {
                foreach (var facility in facilityList)
                {
                    var existingF = await _context.facility.FindAsync(facility.id);

                    if (existingF == null)
                    {
                        // 如果节点不存在，添加它
                        _context.Add(facility);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task linkNodesFromList(List<node> fatherNodeList, List<node> childNodeList)
        {
            if (fatherNodeList.Count != childNodeList.Count)
            {
                // 确保两个列表的长度相同，否则不进行连接
                return;
            }

            for (int i = 0; i < fatherNodeList.Count; i++)
            {
                var fatherNode = fatherNodeList[i];
                var childNode = childNodeList[i];

                var fathernode = await _context.node.FindAsync(fatherNode.id);
                var childnode = await _context.node.FindAsync(childNode.id);

                var fatherRoad = await _context.road.FindAsync(fatherNode.road_id);
                var childRoad = await _context.road.FindAsync(childNode.road_id);

                if (fathernode != null && childnode != null)
                {
                    // 添加子节点的 ID 到父节点的 childnode
                    if (string.IsNullOrEmpty(fathernode.childnode))
                    {
                        fathernode.childnode = childnode.id;
                    }
                    else
                    {
                        fathernode.childnode += ";" + childnode.id;
                    }
                    // 添加父节点的 ID 到子节点的 fathernode
                    if (string.IsNullOrEmpty(childnode.fathernode))
                    {
                        childnode.fathernode = fathernode.id;
                    }
                    else
                    {
                        childnode.fathernode += ";" + fathernode.id;
                    }

                    if (fatherRoad.start_node == fathernode.id)
                    {
                        if (string.IsNullOrEmpty(fatherRoad.start_linknode))
                        {
                            fatherRoad.start_linknode = childnode.id;
                        }
                        else
                        {
                            fatherRoad.start_linknode += ";" + childnode.id;
                        }
                    }
                    else if (fatherRoad.end_node == fathernode.id)
                    {
                        if (string.IsNullOrEmpty(fatherRoad.end_linknode))
                        {
                            fatherRoad.end_linknode = childnode.id;
                        }
                        else
                        {
                            fatherRoad.end_linknode += ";" + childnode.id;
                        }
                    }

                    if (childRoad.start_node == childnode.id)
                    {
                        if (string.IsNullOrEmpty(childRoad.start_linknode))
                        {
                            childRoad.start_linknode = fathernode.id;
                        }
                        else
                        {
                            childRoad.start_linknode += ";" + fathernode.id;
                        }
                    }
                    else if (childRoad.end_node == childnode.id)
                    {
                        if (string.IsNullOrEmpty(childRoad.end_linknode))
                        {
                            childRoad.end_linknode = fathernode.id;
                        }
                        else
                        {
                            childRoad.end_linknode += ";" + fathernode.id;
                        }
                    }

                    // 更新父节点和子节点
                    _context.Update(fathernode);
                    _context.Update(childnode);
                    _context.Update(fatherRoad);
                    _context.Update(childRoad);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task linkNodesFromList(node fatherNode, node childNode)
        {
            var fathernode = await _context.node.FindAsync(fatherNode.id);
            var childnode = await _context.node.FindAsync(childNode.id);

            if (fathernode != null && childnode != null)
            {
                // 添加子节点的 ID 到父节点的 childnode
                if (string.IsNullOrEmpty(fathernode.childnode))
                {
                    fathernode.childnode = childnode.id;
                }
                else
                {
                    fathernode.childnode += ";" + childnode.id;
                }

                // 添加父节点的 ID 到子节点的 fathernode
                if (string.IsNullOrEmpty(childnode.fathernode))
                {
                    childnode.fathernode = fathernode.id;
                }
                else
                {
                    childnode.fathernode += ";" + fathernode.id;
                }

                // 更新父节点和子节点
                _context.Update(fathernode);
                _context.Update(childnode);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportFileAsync(IFormFile file)
        {
            return Json(new { success = false, error = "No file selected" });
        }

        public async Task createHubNodeResFromList(List<HubNodeRes> hubNodeReses)
        {
            if (hubNodeReses != null && hubNodeReses.Count > 0)
            {
                foreach (var res in hubNodeReses)
                {
                    var existingF = await _context.HubNodeRes.FindAsync(res.id);

                    if (existingF == null)
                    {
                        // 如果节点不存在，添加它
                        _context.Add(res);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task createAllNodeResFromList(List<AllNodesRes> allNodesRes)
        {
            if (allNodesRes != null && allNodesRes.Count > 0)
            {
                foreach (var res in allNodesRes)
                {
                    var existingF = await _context.AllNodesRes.FindAsync(res.id);

                    if (existingF == null)
                    {
                        // 如果节点不存在，添加它
                        _context.Add(res);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }


        [HttpGet]
        // 添加文件下载的动作方法
        public ActionResult DownloadFile()
        {
            // 指定文件路径
            string wwwrootPath = _hostingEnvironment.WebRootPath;

            #region HubNodeResult.csv
            // HubNodeResult.csv
            // 获取 HubNodeRes 数据
            var hubNodeResData = (from r in _context.HubNodeRes
                                 select r).ToList();
            // 定义 CSV 文件路径
            var fileName = "HubNodeResult.csv";
            var filePath = Path.Combine(wwwrootPath, fileName);

            // 配置 CSVWriter
            var csvConfig = new CsvConfiguration(new CultureInfo("en-US"))
            {
                Delimiter = ","
            };

            // 创建文件流
            using (var streamWriter = new StreamWriter(filePath))
            using (var csvWriter = new CsvHelper.CsvWriter(streamWriter, csvConfig))
            {
                // 写入表头
                csvWriter.WriteRecords(new List<HubNodeRes> { hubNodeResData.First() });

                // 写入数据
                csvWriter.WriteRecords(hubNodeResData);
            }
            #endregion
            #region AllNodesResult.csv
            // AllNodesResult.csv
            // 获取 AllNodesResult 数据
            var allNodeResData = (from r in _context.AllNodesRes
                                  select r).ToList();
            fileName = "AllNodesResult.csv";
            filePath = Path.Combine(wwwrootPath, fileName);

            // 创建文件流
            using (var streamWriter = new StreamWriter(filePath))
            using (var csvWriter = new CsvHelper.CsvWriter(streamWriter, csvConfig))
            {
                // 写入表头
                csvWriter.WriteRecords(new List<AllNodesRes> { allNodeResData.First() });

                // 写入数据
                csvWriter.WriteRecords(allNodeResData);
            }
            #endregion
            #region Results.xlsx
            fileName = "Results.xlsx";
            filePath = Path.Combine(wwwrootPath, fileName);
            if (System.IO.File.Exists(filePath) == false)
            {
                using (var package = new ExcelPackage(filePath))
                {
                    // 添加第一个工作表（HubNodeRes 数据）
                    var worksheet1 = package.Workbook.Worksheets.Add("HubNodeRes");
                    worksheet1.Cells.LoadFromCollection(hubNodeResData, true, TableStyles.Light9); // 第三个参数添加表头

                    // 添加第二个工作表（AllNodesRes 数据）
                    var worksheet2 = package.Workbook.Worksheets.Add("AllNodesRes");
                    worksheet2.Cells.LoadFromCollection(allNodeResData, true, TableStyles.Light9); // 第三个参数添加表头

                    // 保存 Excel 文件
                    package.Save();
                }
            }
            #endregion

            // download
            filePath = Path.Combine(wwwrootPath, "Results.xlsx");
            // 读取文件内容并存储在字节数组中
            byte[] fileBytes;

            if (System.IO.File.Exists(filePath))
            {
                fileBytes = System.IO.File.ReadAllBytes(filePath);
            }
            else
            {
                // 处理文件不存在的情况，这里可以返回适当的错误消息
                return NotFound(); // 或其他合适的处理方式
            }

            // 设置要下载的文件名
            fileName = "Results.xlsx";

            // 使用 File 方法返回字节数组作为文件下载
            return File(fileBytes, "application/octet-stream", fileName);
        }

        [HttpPost]
        public IActionResult DeleteAllNodes()
        {
            try
            {
                
                _context.Database.ExecuteSqlRaw("DELETE FROM node where id != 'JuRuibin00'");
                _context.Database.ExecuteSqlRaw("DELETE FROM road");
                _context.Database.ExecuteSqlRaw("DELETE from HubNodeRes");
                _context.Database.ExecuteSqlRaw("DELETE from AllNodesRes");

                return RedirectToAction(nameof(BaiduMap));
            }
            catch (Exception ex)
            {
                return BadRequest("操作失败");
            }
        }
    }
}
