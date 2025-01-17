﻿using Fashion_Flex.Models;
using Fashion_Flex.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Fashion_Flex.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            var orders = _orderRepository.GetAll();
            return View(orders);
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Order order)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.Add(order);
                _orderRepository.Save();
                return RedirectToAction("Index");
            }

            return View("New", order);
        }

		[HttpGet]
		public IActionResult Edit(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                _orderRepository.Update(order);
                _orderRepository.Save();
                return RedirectToAction("Index");
            }

            return View("Edit", order);
        }

		[HttpGet]
		public IActionResult Delete(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            _orderRepository.Delete(id);
            _orderRepository.Save();

            return RedirectToAction("Index");
        }


        // Get order history for a customer
        public IActionResult OrdersHistory(int customerId)
        {
            var orders = _orderRepository.GetOrdersByCustomerId(customerId); //Get all the orders a customer made
            return View(orders); //
        }

        // Get details for a specific order
        public IActionResult OrderDetails(int orderId)
        {
            var order = _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order); //
        }

		//Mark Order (As Completed)
		public IActionResult MarkOrderAsCompleted(int orderId)
		{
			bool isUpdatedSuccefully = _orderRepository.updateOrderStatus(orderId, "Completed");

			if (isUpdatedSuccefully)
			{
				return Ok("Order is succecfuly placed");
			} else
            {
				return BadRequest("Order couldn't be placed");
			}
		}
	}
}
