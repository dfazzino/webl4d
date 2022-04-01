$(document).ready(function () {
	$("#myName").on('keyup', function (e) {   //After entering note for the to do task, pressing enter will submit the note to the databaes.
		if (e.which == 13) {
			var data = { "Name": $("#myName").val() }
			$.ajax({
				type: "POST",
				url: "/Index?handler=NewGroup",
				beforeSend: function (xhr) {
					xhr.setRequestHeader("XSRF-TOKEN",
						$('input:hidden[name="__RequestVerificationToken"]').val());
				}, //or
				headers: {
					"XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
				},
				data: data,
				dataType: "html",
				success: function (response) {
					$("#groupsDiv").html(response)
					addListeners();
				},
				error: function (response) {
					//alert(response.d);
				}
			});
		}
	})

	setInterval(function () {
		$.ajax({
			type: "POST",
			url: "/Index?handler=Refresh",
			beforeSend: function (xhr) {
				xhr.setRequestHeader("XSRF-TOKEN",
					$('input:hidden[name="__RequestVerificationToken"]').val());
			}, //or
			headers: {
				"XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
			},
			data: "test=test",
			dataType: "html",
			success: function (response) {
				//alert(response)
			},
			error: function (response) {
				//alert(response.d);
			}
		});

	}, 10000)
	$(".message").removeClass("new");
	function addListeners() {
		$(".ready").on("click", function (e) {
			var data = {
				"GroupID": e.currentTarget.parentElement.parentElement.id
			}
			$.ajax({
				type: "POST",
				url: "/Index?handler=Ready",
				beforeSend: function (xhr) {
					xhr.setRequestHeader("XSRF-TOKEN",
						$('input:hidden[name="__RequestVerificationToken"]').val());
				}, //or
				headers: {
					"XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
				},
				data: data,
				dataType: "html",
				success: function (response) {
					$("#groupsDiv").html(response)
					addListeners();
				},
				error: function (response) {
					//alert(response.d);
				}
			});
		})
		$(".invite").on("click", function (e) {
			var data = {
				"ID": e.currentTarget.parentElement.parentElement.id
			}
			$.ajax({
				type: "POST",
				url: "/Index?handler=Invite",
				beforeSend: function (xhr) {
					xhr.setRequestHeader("XSRF-TOKEN",
						$('input:hidden[name="__RequestVerificationToken"]').val());
				}, //or
				headers: {
					"XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
				},
				data: data,
				dataType: "html",
				success: function (response) {
					$("#messageListDiv").html(response)
					setTimeout(function () {
						$(".message").removeClass("new");
					}, 50)
					//addListeners();
				},
				error: function (response) {
					//alert(response.d);
				}
			});
		})
		$(".join").on("click", function (e) {
			var data = {
				"GroupId": e.currentTarget.parentElement.parentElement.id
			}
			$.ajax({
				type: "POST",
				url: "/Index?handler=JoinGroup",
				beforeSend: function (xhr) {
					xhr.setRequestHeader("XSRF-TOKEN",
						$('input:hidden[name="__RequestVerificationToken"]').val());
				}, //or
				headers: {
					"XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
				},
				data: data,
				dataType: "html",
				success: function (response) {
					$("#groupsDiv").html(response);
					addListeners();
				},
				error: function (response) {
					//alert(response.d);
				}
			});
		})
		$(".leave").on("click", function (e) {
			var data = {
				"Name": $("#myName").val(),
				"GroupID": e.currentTarget.parentElement.parentElement.id
			}
			$.ajax({
				type: "POST",
				url: "/Index?handler=JoinGroup",
				beforeSend: function (xhr) {
					xhr.setRequestHeader("XSRF-TOKEN",
						$('input:hidden[name="__RequestVerificationToken"]').val());
				}, //or
				headers: {
					"XSRF-TOKEN": $('input:hidden[name="__RequestVerificationToken"]').val()
				},
				data: data,
				dataType: "html",
				success: function (response) {
					$("#groupsDiv").html(response);
					addListeners();
				},
				error: function (response) {
					alert(response.d);
				}
			});
		})

	}
	addListeners();
});