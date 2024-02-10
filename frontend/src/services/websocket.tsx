import React, { createContext, useContext, useEffect, useState } from "react";
import { ProgressTask } from "./openapi";

function createWebSocket(callback: (tasks: ProgressTask[]) => void) {
	let reconnect = true;
	let progresses: ProgressTask[] | null = null;
	let webSocket: WebSocket;

	const createConnection = () => {
		let uri: { protocol: string; host: string };

		if (process.env.NEXT_PUBLIC_API_BASE) {
			uri = new URL(process.env.NEXT_PUBLIC_API_BASE);
		} else {
			uri = window.location;
		}

		const protocol = uri.protocol === 'https:' ? 'wss' : 'ws';
		const path = `${protocol}://${uri.host}/api/ws`;

		webSocket = new WebSocket(path);
		webSocket.onclose = onClose;
		webSocket.onmessage = onMessage;
	}

	const onMessage = (event: MessageEvent) => {
		const data = JSON.parse(event.data) as ProgressTask[];

		if (progresses === null) {
			progresses = data.filter((task) => !task.isCompleted);
		} else {
			const receivedIds = data.map((task) => task.id);

			progresses = [
				...progresses.filter((task) => !receivedIds.includes(task.id)),
				...data.filter((task) => !task.isCompleted)
			]
		}

		progresses.sort((a, b) => a.id - b.id);

		callback(progresses);
	};

	const onClose = (e: CloseEvent) => {
		console.log('Socket is closed. Reconnect will be attempted in 1 second.', e);

		if (reconnect) {
			setTimeout(() => {
				createConnection();
			}, 1000);
		}
	};

	createConnection();

	return () => {
		reconnect = false;
		webSocket.close();
	};
}

export const WebSocketState = createContext<ProgressTask[]>([]);

export function WebSocketProvider({children}: { children: React.ReactNode }) {
	const [tasks, setTasks] = useState<ProgressTask[]>([]);

	useEffect(() => {
		return createWebSocket((tasks) => {
			setTasks(tasks);
		});
	}, []);

	return (
		<WebSocketState.Provider value={tasks}>
			{children}
		</WebSocketState.Provider>
	);
}

export function useTasks() {
	return useContext(WebSocketState);
}
